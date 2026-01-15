using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class UserLoginRequest:BaseApiRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public bool isRemember { get; set; }

    }
}
