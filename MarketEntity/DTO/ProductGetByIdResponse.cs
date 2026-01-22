using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class ProductGetByIdResponse:BaseApiResponse
    {
        public long ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        
        public string? CoverBase64 { get; set; }
        public string? CoverContentType { get; set; }
    }
}
