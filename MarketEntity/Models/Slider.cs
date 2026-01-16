using MarketCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Models
{
    [Table("sliders")]
    public class Slider : BaseEntity
    {
        [Column("title", TypeName = "nvarchar(100)")]
        public string? Title { get; set; }

        //Base64 büyük: nvarchar
        [Column("image_base64", TypeName = "nvarchar(max)")]
        public string ImageBase64 { get; set; } = string.Empty;

        [Column("image_content_type", TypeName = "nvarchar(100)")]
        public string ImageContentType { get; set; } = "image/jpeg";

        [Column("image_size_bytes")]
        public long ImageSizeBytes { get; set; }

        [Column("redirect_url", TypeName = "nvarchar(500)")]
        public string RedirectUrl { get; set; } = string.Empty;

        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}
