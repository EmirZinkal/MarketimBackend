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
        private readonly AppDbContext _context;
        public EfNotificationDal(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Notification> GetByUser(int userId, bool? isRead, int skip, int take)
        {
            var q = _context.Notifications.AsNoTracking().Where(n => n.UserId == userId);

            if (isRead.HasValue) q = q.Where(n => n.IsRead == isRead.Value);

            return q.OrderByDescending(n => n.CreatedAt)
                    .Skip(skip).Take(take)
                    .ToList();
        }

        public int GetByUserCount(int userId, bool? isRead)
        {
            var q = _context.Notifications.Where(n => n.UserId == userId);
            if (isRead.HasValue) q = q.Where(n => n.IsRead == isRead.Value);
            return q.Count();
        }
    }
}
