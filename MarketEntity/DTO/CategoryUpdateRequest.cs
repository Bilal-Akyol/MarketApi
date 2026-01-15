using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class CategoryUpdateRequest:BaseApiRequest
    {
        public long ProductId { get; set; }
        public string CategoryName { get; set; }
    }
}
