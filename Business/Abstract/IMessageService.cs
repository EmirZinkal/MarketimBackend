using Core.Utilities.Results;
using Entities.Concrete;
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
    }
}
