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
        public List<FriendDto> GetFriendsWithUserInfo(int userId)
        {
            using var context = new AppDbContext(); // Context oluştur

            // Tek sorgu ile hem kullanıcı hem arkadaş bilgilerini çekiyoruz
            var result =
                from f in context.Friends.AsNoTracking() // Friend tablosu (takipsiz)
                join u in context.Users.AsNoTracking() on f.UserId equals u.Id // Kullanıcı join
                join fr in context.Users.AsNoTracking() on f.FriendId equals fr.Id // Arkadaş join
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




            //    using var context = new AppDbContext(); // Veritabanı context'ini oluşturuyoruz

            //    // 1) Önce userId'ye sahip kullanıcının adını tek seferde çekiyoruz.
            //    //    Böylece her join satırında tekrar tekrar sorgu atmamış oluyoruz.
            //    var userName = context.Set<User>()
            //                          .AsNoTracking() // EF Core'un change tracker'ını devre dışı bırak, performans için
            //                          .Where(u => u.Id == userId) // Filtre: sadece istenen kullanıcı
            //                          .Select(u => u.FullName)    // Sadece FullName kolonunu çek
            //                          .FirstOrDefault();          // Tek satır döndür (null olabilir)

            //    // 2) Kullanıcı bulunamadıysa, arkadaş listesi zaten olamaz.
            //    //    Bu noktada boş liste dönerek metodu bitiriyoruz.
            //    if (userName == null)
            //        return new List<FriendDto>();

            //    // 3) Arkadaş listesini almak için Friend tablosunu User tablosuna join ediyoruz.
            //    //    Join mantığı: Friend.FriendId == User.Id (arkadaşın kullanıcı bilgisi)
            //    var query =
            //        from f in context.Set<Friend>().AsNoTracking() // Friend tablosunu sorgula
            //        where f.UserId == userId                       // Yalnızca belirtilen kullanıcıya ait arkadaşlıklar
            //        join fr in context.Set<User>().AsNoTracking()  // User tablosunu sorguya dahil et
            //             on f.FriendId equals fr.Id                // Join koşulu: FriendId == User.Id
            //        select new FriendDto                           // DTO'ya map işlemi
            //        {
            //            Id = f.Id,         // Friend tablosundaki kayıt Id
            //            UserId = f.UserId,     // Arkadaşlığı başlatan kullanıcı
            //            FriendId = f.FriendId,   // Eklenen arkadaşın Id'si
            //            UserName = userName,     // Yukarıda aldığımız ana kullanıcının adı
            //            FriendName = fr.FullName,  // Join edilen arkadaşın adı
            //            Since = f.CreatedAt   // Arkadaşlık oluşma tarihi
            //        };

            //    // 4) Sonuçları arkadaş ismine göre sıralayıp listeye çeviriyoruz.
            //    return query
            //        .OrderBy(x => x.FriendName) // Alfabetik sıralama
            //        .ToList();                  // IQueryable -> List dönüşümü (sorgu bu noktada çalışır)
        }
        }
}
