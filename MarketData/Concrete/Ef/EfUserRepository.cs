using MarketCore.Data;
using MarketCore.Data.Ef;
using MarketData.Abstract;
using MarketEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketData.Concrete.Ef
{
    public class EfUserRepository : EfEntityRepository<User, MarketDbContext>, IUserRepository
    {
    }
}
