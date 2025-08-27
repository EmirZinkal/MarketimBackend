using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IMessageService
    {
        IResult Add(Message message);
        IResult Update(Message message);
        IResult Delete(Message message);
        IDataResult<Message> GetById(int id);
        IDataResult<List<Message>> GetAll();
        IDataResult<List<MessageDto>> GetConversation(int me, int otherUserId, int page, int pageSize);
        IDataResult<int> GetConversationCount(int me, int otherUserId);

        // Sohbet arama (me <-> other)
        IDataResult<List<MessageDto>> SearchConversation(int me, int otherUserId, string? keyword, DateTime? from, DateTime? to, int page, int pageSize);
        IDataResult<int> SearchConversationCount(int me, int otherUserId, DateTime? from, DateTime? to);

        // Benim tüm mesajlarımda arama (inbox+sent)
        IDataResult<List<MessageDto>> SearchMyMessages(int me, string? keyword, DateTime? from, DateTime? to, int page, int pageSize);
        IDataResult<int> SearchMyMessagesCount(int me, DateTime? from, DateTime? to);
    }
}
