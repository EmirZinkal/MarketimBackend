using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
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
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IUserOperationClaimDal _userOperationClaimDal;


        public UserManager(IUserDal userDal, IUserOperationClaimDal userOperationClaimDal)
        {
            _userDal = userDal;
            _userOperationClaimDal = userOperationClaimDal;
        }

        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult(Messages.UserAdded);
        }

        public IResult Update(User user)
        {
            _userDal.Update(user);
            return new SuccessResult(Messages.UserUpdated);
        }

        public IResult Delete(User user)
        {
            _userDal.Delete(user);
            return new SuccessResult(Messages.UserDeleted);
        }

        public IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Id == id));
        }

        public IDataResult<List<User>> GetAll()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetList().ToList());
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(_userDal.GetClaims(user));
        }

        public IDataResult<User> GetByMail(string email)
        {
            return new SuccessDataResult<User>(_userDal.Get(filter: u => u.Email == email));
        }
        public IResult AssignRole(int userId, int roleId)
        {
            var user = _userDal.Get(u => u.Id == userId);
            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            var existingClaim = _userOperationClaimDal.Get(uoc => uoc.UserId == userId);
            if (existingClaim != null)
            {
                if (existingClaim.RoleId == roleId)
                    return new ErrorResult(Messages.UserAlreadyHasThisRole);

                existingClaim.RoleId = roleId;
                _userOperationClaimDal.Update(existingClaim);
                return new SuccessResult(Messages.UserRoleUpdated);
            }

            _userOperationClaimDal.Add(new UserOperationClaim
            {
                UserId = userId,
                RoleId = roleId
            });

            return new SuccessResult(Messages.RoleAdded);
        }
    }
}
