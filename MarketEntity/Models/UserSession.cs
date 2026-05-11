using MarketCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Models
{
    [Table("user_sessions")]
    public class UserSession : BaseEntity
    {
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("session_token")]
        public string SessionToken { get; set; }

        [Column("expire_at")]
        public DateTime ExpireAt { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("ip_address")]
        public string IpAddress { get; set; }

        [Column("user_agent")]
        public string UserAgent { get; set; }
    }
}