using MarketCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketEntity.Models
{
    [Table("categories")]
    public class Categories:BaseEntity
    {
        [Column("name")]
        public string CategoryName { get; set; }
    }
}
