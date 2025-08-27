using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IMessageDal:IEntityRepository<Message>
    {
        // İki kullanıcı arasındaki sohbeti (tek yöne bağlı kalmadan) getirir
        List<Message> GetConversation(int userAId, int userBId, int skip, int take);
        int GetConversationCount(int userAId, int userBId);

        // İki kullanıcı arasındaki sohbet (tarih aralığı + sayfalama)
        List<Message> GetConversationRange(int userAId, int userBId, DateTime? from, DateTime? to, int skip, int take);
        int GetConversationRangeCount(int userAId, int userBId, DateTime? from, DateTime? to);

        // Kullanıcının tüm mesajları (inbox+sent) – tarih aralığı + sayfalama
        List<Message> GetByUserRange(int userId, DateTime? from, DateTime? to, int skip, int take);
        int GetByUserRangeCount(int userId, DateTime? from, DateTime? to);
    }
}
