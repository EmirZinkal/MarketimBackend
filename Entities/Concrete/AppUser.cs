using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class AppUser:User
    {
        // Navigation
        public ICollection<Product> Products { get; set; }
        public ICollection<ShoppingList> ShoppingLists { get; set; }
        public ICollection<Friend> Friends { get; set; }
        public ICollection<Friend> FriendOf { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
