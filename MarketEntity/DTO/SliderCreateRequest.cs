using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class SliderCreateRequest : BaseApiRequest
    {
        public string Title { get; set; }
        public string RedirectUrl { get; set; }

        // Slider görseli Base64 DB’de
        public string ImageBase64 { get; set; }
        public string ImageContentType { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
