using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfFriendDal : EfEntityRepositoryBase<Friend, AppDbContext>, IFriendDal
    {
        private readonly AppDbContext _context;
        public EfFriendDal(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<FriendDto> GetFriendsWithUserInfo(int userId)
        {
            // Tek sorgu ile hem kullanıcı hem arkadaş bilgilerini çekiyoruz
            var result =
                from f in _context.Friends.AsNoTracking() // Friend tablosu (takipsiz)
                join u in _context.Users.AsNoTracking() on f.UserId equals u.Id // Kullanıcı join
                join fr in _context.Users.AsNoTracking() on f.FriendId equals fr.Id // Arkadaş join
                where f.UserId == userId // Sadece belirtilen kullanıcının arkadaşları
                select new FriendDto
                {
                    Id = f.Id,               // Friend tablosundaki kayıt Id
                    UserId = f.UserId,       // Kullanıcı Id
                    FriendId = f.FriendId,   // Arkadaş Id
                    UserName = u.FullName,   // Kullanıcı adı (join'den)
                    FriendName = fr.FullName,// Arkadaş adı (join'den)
                    Since = f.CreatedAt      // Arkadaşlık tarihi
                };

            // Alfabetik olarak arkadaş ismine göre sıralayıp liste döndürüyoruz
            return result
                .OrderBy(x => x.FriendName)
                .ToList();
        }
    }
}
