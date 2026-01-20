using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.DTO
{
    public class CategoryListItem
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
