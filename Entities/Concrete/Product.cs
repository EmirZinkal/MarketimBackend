using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }

        public int MarketId { get; set; }
        public Market Market { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
