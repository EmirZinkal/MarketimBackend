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
        void Add(Notification notification);
        void Update(Notification notification);
        void Delete(Notification notification);
        Notification GetById(int id);
        List<Notification> GetAll();
    }
}
