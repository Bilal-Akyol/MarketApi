using MarketCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Models
{
    [Table("product")]
    public class Product : BaseEntity
    {

        [Column("name")]
        public string? Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        
        public int Stock { get; set; }

        public string? ProductImage { get; set; }


        public bool IsActive { get; set; } = true;

        
        public long CategoryId { get; set; }
    }
}
