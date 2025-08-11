using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos.Auth
{
    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; } = null!;
    }
}
