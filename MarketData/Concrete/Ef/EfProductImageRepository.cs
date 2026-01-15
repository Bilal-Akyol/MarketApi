using MarketCore.Data.Ef;
using MarketData.Abstract;
using MarketEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Concrete.Ef
{
    public class EfProductImageRepository:EfEntityRepository<ProductImage,MarketDbContext>,IProductImageRepository
    {
    }
}
