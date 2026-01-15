using MarketCore.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Models
{
    [Table("user")]
    public class User:BaseEntity
    {
        [Column("first_name")]
        public string FirstName { get; set; }


        [Column("last_name")]
        public string LastName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("email_confirmed")]
        public bool EmailConfirmed { get; set; }

        [Column("ip")]
        public string Ip { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("role_id")]
        public long RoleId { get; set; }

        [Column("Remember")]
        [NotMapped]
        public bool Remember { get; set; }
    }
}
