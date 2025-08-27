using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface INotificationDal:IEntityRepository<Notification>
    {
        List<Notification> GetByUser(int userId, bool? isRead, int skip, int take);
        int GetByUserCount(int userId, bool? isRead);
    }
}
