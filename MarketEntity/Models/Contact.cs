using MarketCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Models
{
    [Table("contact")]
    public class Contact : BaseEntity
    {
        [Column("title", TypeName = "nvarchar(150)")]
        public string Title { get; set; } = string.Empty;

        [Column("content", TypeName = "nvarchar(max)")]
        public string Content { get; set; } = string.Empty;

        [Column("phone", TypeName = "nvarchar(30)")]
        public string Phone { get; set; } = string.Empty;

        [Column("email", TypeName = "nvarchar(150)")]
        public string Email { get; set; } = string.Empty;

        [Column("address", TypeName = "nvarchar(500)")]
        public string Address { get; set; } = string.Empty;

        // opsiyonel: harita iframe linki veya google maps url
        [Column("map_url", TypeName = "nvarchar(500)")]
        public string? MapUrl { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}
