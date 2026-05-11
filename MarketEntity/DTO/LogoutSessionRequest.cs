using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class LogoutSessionRequest : BaseApiRequest
    {
        public long SessionId { get; set; }
    }
}
