using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Infrastructure.Persistence;

namespace SystoQ.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SystoQDbContext context) : base(context)
        {            
        }
    }
}
