using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.Dtos;
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
        private readonly IUserDal _userDal;

        public FriendManager(IFriendDal friendDal, IUserDal userDal)
        {
            _friendDal = friendDal;
            _userDal = userDal;
        }
        [ValidationAspect(typeof(FriendValidator))]
        public IResult Add(Friend friend)
        {
            // Kullanıcı var mı?
            var userExists = _userDal.Get(u => u.Id == friend.UserId);
            if (userExists == null)
            {
                return new ErrorResult(Messages.FriendUserIdRequired);
            }

            // Arkadaş var mı?
            var friendUserExists = _userDal.Get(u => u.Id == friend.FriendId);
            if (friendUserExists == null)
            {
                return new ErrorResult(Messages.FriendFriendIdRequired);
            }

            // Kendini arkadaş olarak ekleyemez
            if (friend.UserId == friend.FriendId)
            {
                return new ErrorResult(Messages.FriendCannotAddSelf);
            }

            // Zaten ekli mi?
            var alreadyFriends = _friendDal.Get(f => f.UserId == friend.UserId && f.FriendId == friend.FriendId);
            if (alreadyFriends != null)
            {
                return new ErrorResult(Messages.FriendAlreadyExists);
            }

            friend.CreatedAt = DateTime.Now;

            _friendDal.Add(friend);
            return new SuccessResult(Messages.FriendAdded);
        }

        public IResult Delete(Friend friend)
        {
            var existingFriend = _friendDal.Get(f => f.Id == friend.Id);
            if (existingFriend == null)
            {
                return new ErrorResult(Messages.FriendNotFound);
            }

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

        public IDataResult<List<FriendDto>> GetFriendsWithUserInfo(int userId)
        {
            var list = _friendDal.GetFriendsWithUserInfo(userId);
            return new SuccessDataResult<List<FriendDto>>(list);
        }

        [ValidationAspect(typeof(FriendValidator))]
        public IResult Update(Friend friend)
        {
            // Kayıt var mı?
            var existingFriend = _friendDal.Get(f => f.Id == friend.Id);
            if (existingFriend == null)
            {
                return new ErrorResult(Messages.FriendNotFound);
            }

            // Kullanıcı var mı?
            var userExists = _userDal.Get(u => u.Id == friend.UserId);
            if (userExists == null)
            {
                return new ErrorResult(Messages.FriendUserIdRequired);
            }

            // Arkadaş var mı?
            var friendUserExists = _userDal.Get(u => u.Id == friend.FriendId);
            if (friendUserExists == null)
            {
                return new ErrorResult(Messages.FriendFriendIdRequired);
            }

            // Kendini arkadaş olarak ekleyemez
            if (friend.UserId == friend.FriendId)
            {
                return new ErrorResult(Messages.FriendCannotAddSelf);
            }

            _friendDal.Update(friend);
            return new SuccessResult(Messages.FriendUpdated);
        }
    }
}
