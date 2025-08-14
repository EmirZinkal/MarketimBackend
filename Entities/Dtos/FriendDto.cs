using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class FriendDto :IDto
    {
        public int Id { get; set; }      
        public int UserId { get; set; }      
        public int FriendId { get; set; }    
        public string UserName { get; set; } = null!;
        public string FriendName { get; set; } = null!;
        public DateTime Since { get; set; } 
    }
}
