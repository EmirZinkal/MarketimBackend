using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class NotificationManager : INotificationService
    {
        private readonly INotificationDal _notificationDal;

        public NotificationManager(INotificationDal notificationDal)
        {
            _notificationDal = notificationDal;
        }

        public IResult Add(Notification notification)
        {
            _notificationDal.Add(notification);
            return new SuccessResult(Messages.NotificationAdded);
        }

        public IResult Delete(Notification notification)
        {
            _notificationDal.Delete(notification);
            return new SuccessResult(Messages.NotificationDeleted);
        }

        public IDataResult<List<Notification>> GetAll()
        {
            return new SuccessDataResult<List<Notification>>(_notificationDal.GetList().ToList());
        }

        public IDataResult<Notification> GetById(int id)
        {
            return new SuccessDataResult<Notification>(_notificationDal.Get(u=>u.Id == id));
        }

        public IResult Update(Notification notification)
        {
            _notificationDal.Update(notification);
            return new SuccessResult(Messages.NotificationUpdated);
        }

        public IDataResult<List<Notification>> GetMyNotifications(int userId, bool? isRead, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 20;

            var skip = (page - 1) * pageSize;
            var list = _notificationDal.GetByUser(userId, isRead, skip, pageSize);
            return new SuccessDataResult<List<Notification>>(list);
        }

        public IDataResult<int> GetMyNotificationsCount(int userId, bool? isRead)
        {
            var count = _notificationDal.GetByUserCount(userId, isRead);
            return new SuccessDataResult<int>(count);
        }

        public IResult MarkAsRead(int id, int userId)
        {
            var n = _notificationDal.Get(x => x.Id == id && x.UserId == userId);
            if (n == null) return new ErrorResult("Bildirim bulunamadı.");

            n.IsRead = true;
            _notificationDal.Update(n);
            return new SuccessResult("Bildirim okundu olarak işaretlendi.");
        }

        public IResult MarkAllAsRead(int userId)
        {
            var list = _notificationDal.GetList(n => n.UserId == userId && !n.IsRead).ToList();
            if (list.Count == 0) return new SuccessResult("Okunmamış bildirim yok.");

            foreach (var n in list)
            {
                n.IsRead = true;
                _notificationDal.Update(n); 
            }
            return new SuccessResult("Tüm bildirimler okundu.");
        }
    }
}
