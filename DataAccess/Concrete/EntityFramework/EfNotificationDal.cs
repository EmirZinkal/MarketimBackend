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
    public class EfNotificationDal : EfEntityRepositoryBase<Notification, AppDbContext>, INotificationDal
    {
        public List<Notification> GetByUser(int userId, bool? isRead, int skip, int take)
        {
            using var ctx = new AppDbContext();
            var q = ctx.Notifications.AsNoTracking().Where(n => n.UserId == userId);

            if (isRead.HasValue) q = q.Where(n => n.IsRead == isRead.Value);

            return q.OrderByDescending(n => n.CreatedAt)
                    .Skip(skip).Take(take)
                    .ToList();
        }

        public int GetByUserCount(int userId, bool? isRead)
        {
            using var ctx = new AppDbContext();
            var q = ctx.Notifications.Where(n => n.UserId == userId);
            if (isRead.HasValue) q = q.Where(n => n.IsRead == isRead.Value);
            return q.Count();
        }
    }
}
