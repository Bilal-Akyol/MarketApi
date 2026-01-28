using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class LogoCreateRequest : BaseApiRequest
    {
        public string? Title { get; set; }
        public string ImageBase64 { get; set; } = string.Empty;
        public string ImageContentType { get; set; } = "image/png";
        public bool IsActive { get; set; } = true;
    }
}
