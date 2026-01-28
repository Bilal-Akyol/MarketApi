using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class LogoListResponse : BaseApiResponse
    {
        public List<LogoListItem> Logos { get; set; } = new List<LogoListItem>();
    }
}
