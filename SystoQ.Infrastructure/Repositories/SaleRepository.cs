using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Infrastructure.Persistence;

namespace SystoQ.Infrastructure.Repositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        public SaleRepository(SystoQDbContext context) : base(context)
        {            
        }
    }
}
