using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class ProductListResponse : BaseApiResponse
    {
        public List<ProductListItem> Products { get; set; } = new();

    }
}
