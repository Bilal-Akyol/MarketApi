using MarketCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Models
{
    [Table("product_image")]
    public class ProductImage : BaseEntity
    {
        [Column("product_id")]
        public long ProductId { get; set; }

        // Base64 çok büyük olacağı için nvarchar şart
        [Column("base64", TypeName = "nvarchar(max)")]
        public string Base64 { get; set; } = string.Empty;

        [Column("content_type", TypeName = "nvarchar(100)")]
        public string ContentType { get; set; } = "image/jpeg";

        [Column("size_bytes")]
        public long SizeBytes { get; set; }

        [Column("is_cover")]
        public bool IsCover { get; set; } = false;
    }
}
