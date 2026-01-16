using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class SliderUpdateRequest : BaseApiRequest
    {
        public long SliderId { get; set; }   // şirkette genelde request içinde id olur

        public string Title { get; set; }
        public string RedirectUrl { get; set; }

        // Resim güncellemek istersen gönder 
        public string? ImageBase64 { get; set; }
        public string? ImageContentType { get; set; }


        public bool IsActive { get; set; } = true;
    }
}
