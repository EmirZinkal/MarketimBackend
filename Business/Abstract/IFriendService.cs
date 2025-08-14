using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IFriendService
    {
        IResult Add(Friend friend);
        IResult Update(Friend friend);
        IResult Delete(Friend friend);
        IDataResult<Friend> GetById(int id);
        IDataResult<List<Friend>> GetAll();
        IDataResult<List<FriendDto>> GetFriendsWithUserInfo(int userId);
    }
}
