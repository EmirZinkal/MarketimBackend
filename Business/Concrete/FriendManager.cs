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
    public class FriendManager : IFriendService
    {
        private readonly IFriendDal _friendDal;

        public FriendManager(IFriendDal friendDal)
        {
            _friendDal = friendDal;
        }
        public IResult Add(Friend friend)
        {
            _friendDal.Add(friend);
            return new SuccessResult(Messages.FriendAdded);
        }

        public IResult Delete(Friend friend)
        {
            _friendDal.Delete(friend);
            return new SuccessResult(Messages.FriendDeleted);
        }

        public IDataResult<List<Friend>> GetAll()
        {
            return new SuccessDataResult<List<Friend>>(_friendDal.GetList().ToList());
        }

        public IDataResult<Friend> GetById(int id)
        {
            return new SuccessDataResult<Friend>(_friendDal.Get(u=>u.Id == id));
        }

        public IResult Update(Friend friend)
        {
            _friendDal.Update(friend);
            return new SuccessResult(Messages.FriendUpdated);
        }
    }
}
