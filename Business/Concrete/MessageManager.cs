using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }
        [ValidationAspect(typeof(MessageValidator))]
        public IResult Add(Message message)
        {
            _messageDal.Add(message);
            return new SuccessResult(Messages.MessageAdded);
        }
        [ValidationAspect(typeof(MessageValidator))]
        public IResult Update(Message message)
        {
            _messageDal.Update(message);
            return new SuccessResult(Messages.MessageUpdated);
        }

        public IResult Delete(Message message)
        {
            _messageDal.Delete(message);
            return new SuccessResult(Messages.MessageDeleted);
        }

        public IDataResult<Message> GetById(int id)
        {
            return new SuccessDataResult<Message>(_messageDal.Get(u => u.Id == id));
        }

        public IDataResult<List<Message>> GetAll()
        {
            return new SuccessDataResult<List<Message>>(_messageDal.GetList().ToList());
        }
    }
}
