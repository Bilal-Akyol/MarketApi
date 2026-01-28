using MarketCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Models
{
    [Table("logos")]
    public class Logo : BaseEntity
    {
        [Column("title", TypeName = "nvarchar(100)")]
        public string? Title { get; set; }

        [Column("image_base64", TypeName = "nvarchar(max)")]
        public string ImageBase64 { get; set; } = string.Empty;

        [Column("image_content_type", TypeName = "nvarchar(100)")]
        public string ImageContentType { get; set; } = "image/png";

        [Column("image_size_bytes")]
        public long ImageSizeBytes { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}
