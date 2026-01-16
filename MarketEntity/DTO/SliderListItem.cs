using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class SliderListItem
    {
        public long SliderId { get; set; }
        public string Title { get; set; }
        public string RedirectUrl { get; set; }

        public string ImageBase64 { get; set; }
        public string ImageContentType { get; set; }

    }
}
