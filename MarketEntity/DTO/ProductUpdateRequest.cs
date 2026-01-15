using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class ProductUpdateRequest : BaseApiRequest
    {
        public long ProductId { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }

        
        public long CategoryId { get; set; }

        
        public List<Base64ImageDto>? Photos { get; set; }
    }
}
