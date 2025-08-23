using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Infrastructure.Persistence;

namespace SystoQ.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly SystoQDbContext _context;

        public SaleRepository(SystoQDbContext context)
        {
            _context = context;
        }

        public async Task<Sale> AddSaleAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            return sale;
        }
    }
}
