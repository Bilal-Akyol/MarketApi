using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class AboutUpdateRequest : BaseApiRequest
    {
        public long UserId { get; set; }
        public long AboutId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        //gönderilirse güncellensin, gönderilmezse eski kalsın
        public string? ImageBase64 { get; set; }
        public string? ImageContentType { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
