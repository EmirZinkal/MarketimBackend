using Core.Entities;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class PasswordResetToken:IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TokenHash { get; set; } = null!;
        public DateTime ExpireAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public string? IpAddress { get; set; }

        public User User { get; set; } = null!;
    }
}
