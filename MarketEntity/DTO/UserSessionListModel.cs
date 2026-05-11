using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class UserSessionListModel
    {
        public long SessionId { get; set; }
        public string SessionToken { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public DateTime ExpireAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
