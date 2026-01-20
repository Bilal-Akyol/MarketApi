using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class AboutCreateRequest : BaseApiRequest
    {
        public long UserId { get; set; } // controller token'dan set edecek

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        // image şart
        public string ImageBase64 { get; set; } = string.Empty;
        public string ImageContentType { get; set; } = "image/jpeg";

        public bool IsActive { get; set; } = true;
    }
}
