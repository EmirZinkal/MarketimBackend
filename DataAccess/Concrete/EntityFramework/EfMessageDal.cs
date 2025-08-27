using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfMessageDal : EfEntityRepositoryBase<Message, AppDbContext>, IMessageDal
    {
        public List<Message> GetConversation(int userAId, int userBId, int skip, int take)
        {
            using var ctx = new AppDbContext();

            // A<->B arası tüm mesajlar (gönderici/alıcı değişebilir)
            var query = ctx.Messages
                .AsNoTracking()
                .Where(m =>
                    (m.SenderId == userAId && m.ReceiverId == userBId) ||
                    (m.SenderId == userBId && m.ReceiverId == userAId))
                .OrderByDescending(m => m.SentAt) // Yeni mesajlar önce
                .Skip(skip)
                .Take(take);

            return query.ToList();
        }

        public int GetConversationCount(int userAId, int userBId)
        {
            using var ctx = new AppDbContext();

            return ctx.Messages
                .AsNoTracking()
                .Count(m =>
                    (m.SenderId == userAId && m.ReceiverId == userBId) ||
                    (m.SenderId == userBId && m.ReceiverId == userAId));
        }

        public List<Message> GetConversationRange(int userAId, int userBId, DateTime? from, DateTime? to, int skip, int take)
        {
            using var ctx = new AppDbContext();

            var q = ctx.Messages.AsNoTracking()
                .Where(m => (m.SenderId == userAId && m.ReceiverId == userBId) ||
                            (m.SenderId == userBId && m.ReceiverId == userAId));

            if (from.HasValue) q = q.Where(m => m.SentAt >= from.Value);
            if (to.HasValue) q = q.Where(m => m.SentAt <= to.Value);

            return q.OrderByDescending(m => m.SentAt)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
        }

        public int GetConversationRangeCount(int userAId, int userBId, DateTime? from, DateTime? to)
        {
            using var ctx = new AppDbContext();

            var q = ctx.Messages.AsNoTracking()
                .Where(m => (m.SenderId == userAId && m.ReceiverId == userBId) ||
                            (m.SenderId == userBId && m.ReceiverId == userAId));

            if (from.HasValue) q = q.Where(m => m.SentAt >= from.Value);
            if (to.HasValue) q = q.Where(m => m.SentAt <= to.Value);

            return q.Count();
        }

        public List<Message> GetByUserRange(int userId, DateTime? from, DateTime? to, int skip, int take)
        {
            using var ctx = new AppDbContext();

            var q = ctx.Messages.AsNoTracking()
                .Where(m => m.SenderId == userId || m.ReceiverId == userId);

            if (from.HasValue) q = q.Where(m => m.SentAt >= from.Value);
            if (to.HasValue) q = q.Where(m => m.SentAt <= to.Value);

            return q.OrderByDescending(m => m.SentAt)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
        }

        public int GetByUserRangeCount(int userId, DateTime? from, DateTime? to)
        {
            using var ctx = new AppDbContext();

            var q = ctx.Messages.AsNoTracking()
                .Where(m => m.SenderId == userId || m.ReceiverId == userId);

            if (from.HasValue) q = q.Where(m => m.SentAt >= from.Value);
            if (to.HasValue) q = q.Where(m => m.SentAt <= to.Value);

            return q.Count();
        }
    }
}
