using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ShoppingList:IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }
        public string ItemName { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
