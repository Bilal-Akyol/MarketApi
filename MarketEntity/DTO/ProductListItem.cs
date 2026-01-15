using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class ProductListItem
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Kapak foto Base64 (home’da göstermek için)
        public string? CoverBase64 { get; set; }
        public string? CoverContentType { get; set; }
    }
}
