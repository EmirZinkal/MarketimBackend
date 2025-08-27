using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface INotificationService
    {
        IResult Add(Notification notification);
        IResult Update(Notification notification);
        IResult Delete(Notification notification);
        IDataResult<Notification> GetById(int id);
        IDataResult<List<Notification>> GetAll();

        IDataResult<List<Notification>> GetMyNotifications(int userId, bool? isRead, int page, int pageSize);
        IDataResult<int> GetMyNotificationsCount(int userId, bool? isRead);
        IResult MarkAsRead(int id, int userId);
        IResult MarkAllAsRead(int userId);
    }
}
