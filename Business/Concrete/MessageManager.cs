using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
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

        public MessageManager(IMessageDal messageDal, IUserDal userDal)
        {
            _messageDal = messageDal;
            _userDal = userDal;
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

            _messageDal.Add(message);
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

            _messageDal.Update(message);
            return new SuccessResult(Messages.MessageUpdated);
        }

        public IResult Delete(Message message)
        {
            var existingMessage = _messageDal.Get(m => m.Id == message.Id);
            if (existingMessage == null)
            {
                return new ErrorResult(Messages.MessageNotFound);
            }

            _messageDal.Delete(message);
            return new SuccessResult(Messages.MessageDeleted);
        }

        public IDataResult<Message> GetById(int id)
        {
            return new SuccessDataResult<Message>(_messageDal.Get(u => u.Id == id));
        }

        public IDataResult<List<Message>> GetAll()
        {
            return new SuccessDataResult<List<Message>>(_messageDal.GetList().ToList());
        }
    }
}
