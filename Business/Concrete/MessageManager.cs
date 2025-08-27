using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;
        private readonly IUserDal _userDal;
        private readonly INotificationService _notificationService;
        private readonly byte[] _cryptoKey; // DI'dan gelen AES-256 key

        public MessageManager(IMessageDal messageDal, IUserDal userDal, INotificationService notificationService, byte[] cryptoKey)
        {
            _messageDal = messageDal;
            _userDal = userDal;
            _notificationService = notificationService;
            _cryptoKey = cryptoKey;
        }
        [ValidationAspect(typeof(MessageValidator))]
        public IResult Add(Message message)
        {
            // Gönderici var mı kontrolü
            var senderExists = _userDal.Get(u => u.Id == message.SenderId);
            if (senderExists == null)
            {
                return new ErrorResult(Messages.MessageSenderIdRequired);
            }

            // Alıcı var mı kontrolü
            var receiverExists = _userDal.Get(u => u.Id == message.ReceiverId);
            if (receiverExists == null)
            {
                return new ErrorResult(Messages.MessageReceiverIdRequired);
            }

            // Gönderici ve alıcı aynı olamaz
            if (message.SenderId == message.ReceiverId)
            {
                return new ErrorResult(Messages.MessageSenderReceiverCannotBeSame);
            }


            // İki kullanıcı arasında çok kısa sürede tekrar mesaj atılmasını engelleme
            var lastMessage = _messageDal.GetList(m =>
                m.SenderId == message.SenderId &&
                m.ReceiverId == message.ReceiverId)
                .OrderByDescending(m => m.SentAt) // tarih alanı varsa
                .FirstOrDefault();

            if (lastMessage != null && lastMessage.SentAt > DateTime.Now.AddSeconds(-5))
                return new ErrorResult(Messages.MessageSendTooFrequently);


            message.SentAt = DateTime.Now;

            // Düz metni veritabanına yazmadan önce şifrele
            message.MessageText = HashingHelper.EncryptMessage(message.MessageText, _cryptoKey);

            _messageDal.Add(message);

            // Bildirim oluşturma
            var notification = new Notification
            {
                UserId = message.ReceiverId, // Bildirimi alacak kişi
                Message = $"{senderExists.FullName} size yeni bir mesaj gönderdi.",
                CreatedAt = DateTime.Now,
                IsRead = false
            };
            _notificationService.Add(notification);

            return new SuccessResult(Messages.MessageAdded);
        }
        [ValidationAspect(typeof(MessageValidator))]
        public IResult Update(Message message)
        {
            // Mesaj var mı kontrolü
            var existingMessage = _messageDal.Get(m => m.Id == message.Id);
            if (existingMessage == null)
            {
                return new ErrorResult(Messages.MessageNotFound);
            }

            // Gönderici var mı kontrolü
            var senderExists = _userDal.Get(u => u.Id == message.SenderId);
            if (senderExists == null)
            {
                return new ErrorResult(Messages.MessageSenderIdRequired);
            }

            // Alıcı var mı kontrolü
            var receiverExists = _userDal.Get(u => u.Id == message.ReceiverId);
            if (receiverExists == null)
            {
                return new ErrorResult(Messages.MessageReceiverIdRequired);
            }

            // Gönderici ve alıcı aynı olamaz
            if (message.SenderId == message.ReceiverId)
            {
                return new ErrorResult(Messages.MessageSenderReceiverCannotBeSame);
            }

            // Güncellenen metni tekrar şifrele
            message.MessageText = HashingHelper.EncryptMessage(message.MessageText, _cryptoKey);
            message.SentAt = DateTime.Now;
            _messageDal.Update(message);
            return new SuccessResult(Messages.MessageUpdated);
        }

        public IResult Delete(Message message)
        {
            var existingMessage = _messageDal.Get(m => m.Id == message.Id);
            //if (existingMessage == null)
            //{
            //    return new ErrorResult(Messages.MessageNotFound);
            //}

            _messageDal.Delete(existingMessage);
            return new SuccessResult(Messages.MessageDeleted);
        }

        public IDataResult<Message> GetById(int id)
        {
            var msg = _messageDal.Get(u => u.Id == id);
            if (msg == null) return new ErrorDataResult<Message>(Messages.MessageNotFound);

            try
            {
                // Şifrelenmiş metni çözüp düz metin olarak modelde geri göster
                msg.MessageText = HashingHelper.DecryptMessage(msg.MessageText, _cryptoKey);
            }
            catch
            {
                msg.MessageText = "[decryption-error]";
            }

            return new SuccessDataResult<Message>(msg);
        }

        public IDataResult<List<Message>> GetAll()
        {
            var list = _messageDal.GetList().ToList();
            foreach (var m in list)
            {
                try { m.MessageText = HashingHelper.DecryptMessage(m.MessageText, _cryptoKey); }
                catch { m.MessageText = "[decryption-error]"; }
            }
            return new SuccessDataResult<List<Message>>(list);

            //return new SuccessDataResult<List<Message>>(_messageDal.GetList().ToList());
        }

        // Sohbet geçmişi (sayfalı)
        public IDataResult<List<MessageDto>> GetConversation(int me, int otherUserId, int page, int pageSize)
        {
            // Basit guard: minimum değerler
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var skip = (page - 1) * pageSize;

            // DAL’den şifreli entity’leri çek
            var encList = _messageDal.GetConversation(me, otherUserId, skip, pageSize);

            // Her birini decrypt edip DTO’ya dönüştür
            var dto = new List<MessageDto>(encList.Count);
            foreach (var m in encList)
            {
                string plain;
                try { plain = HashingHelper.DecryptMessage(m.MessageText, _cryptoKey); }
                catch { plain = "[decryption-error]"; }

                dto.Add(new MessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    MessageText = plain,
                    SentAt = m.SentAt
                });
            }

            // İstemci genelde ekranda eskiden yeniye görmek ister → İstersen burada yeniden tersine çevir
            // dto = dto.OrderBy(x => x.SentAt).ToList();

            return new SuccessDataResult<List<MessageDto>>(dto);
        }

        public IDataResult<int> GetConversationCount(int me, int otherUserId)
        {
            var count = _messageDal.GetConversationCount(me, otherUserId);
            return new SuccessDataResult<int>(count);
        }

        public IDataResult<List<MessageDto>> SearchConversation(int me, int otherUserId, string? keyword, DateTime? from, DateTime? to, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 20;
            var skip = (page - 1) * pageSize;

            // 1) Tarih aralığına göre şifreli kayıtları çek
            var enc = _messageDal.GetConversationRange(me, otherUserId, from, to, skip, pageSize);

            // 2) Decrypt + keyword filtre (varsa)
            var list = new List<MessageDto>(enc.Count);
            foreach (var m in enc)
            {
                string plain;
                try { plain = HashingHelper.DecryptMessage(m.MessageText, _cryptoKey); }
                catch { plain = "[decryption-error]"; }

                // keyword yoksa hepsini al
                if (string.IsNullOrWhiteSpace(keyword) ||
                    (plain?.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) >= 0))
                {
                    list.Add(new MessageDto
                    {
                        Id = m.Id,
                        SenderId = m.SenderId,
                        ReceiverId = m.ReceiverId,
                        MessageText = plain,
                        SentAt = m.SentAt
                    });
                }
            }

            // İstersen eski → yeni sıraya çevir
            // list = list.OrderBy(x => x.SentAt).ToList();

            return new SuccessDataResult<List<MessageDto>>(list);
        }

        public IDataResult<int> SearchConversationCount(int me, int otherUserId, DateTime? from, DateTime? to)
        {
            // DİKKAT: DB'de keyword filtreleyemiyoruz; bu toplam yalnızca tarih aralığı sayısıdır.
            // UI'da keyword varsa "yaklaşık" sayım olur (sayfada gösterilen gerçek sonuçlar değişebilir).
            var count = _messageDal.GetConversationRangeCount(me, otherUserId, from, to);
            return new SuccessDataResult<int>(count);
        }

        public IDataResult<List<MessageDto>> SearchMyMessages(int me, string? keyword, DateTime? from, DateTime? to, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 20;
            var skip = (page - 1) * pageSize;

            var enc = _messageDal.GetByUserRange(me, from, to, skip, pageSize);

            var list = new List<MessageDto>(enc.Count);
            foreach (var m in enc)
            {
                string plain;
                try { plain = HashingHelper.DecryptMessage(m.MessageText, _cryptoKey); }
                catch { plain = "[decryption-error]"; }

                if (string.IsNullOrWhiteSpace(keyword) ||
                    (plain?.IndexOf(keyword, StringComparison.CurrentCultureIgnoreCase) >= 0))
                {
                    list.Add(new MessageDto
                    {
                        Id = m.Id,
                        SenderId = m.SenderId,
                        ReceiverId = m.ReceiverId,
                        MessageText = plain,
                        SentAt = m.SentAt
                    });
                }
            }

            return new SuccessDataResult<List<MessageDto>>(list);
        }

        public IDataResult<int> SearchMyMessagesCount(int me, DateTime? from, DateTime? to)
        {
            var count = _messageDal.GetByUserRangeCount(me, from, to);
            return new SuccessDataResult<int>(count);
        }
    }
}
