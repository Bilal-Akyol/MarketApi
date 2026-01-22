using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class AboutListItem:BaseApiResponse
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;


        public string ImageBase64 { get; set; }
        public string ImageContentType { get; set; }
    }
}
