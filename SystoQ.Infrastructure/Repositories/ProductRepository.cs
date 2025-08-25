using Microsoft.EntityFrameworkCore;
using SystoQ.Domain.Common.Enums;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Products;
using SystoQ.Domain.Repositories;
using SystoQ.Infrastructure.Persistence;
using X.PagedList;

namespace SystoQ.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SystoQDbContext _context;

        public ProductRepository(SystoQDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            return _context.Products.FindAsync(id).AsTask();
        }

        public async Task<IPagedList<Product>?> GetProductsPriceFilterAsync(ProductsPriceFilter filter)
        {
            var query = _context.Products.AsQueryable(); // DbSet<Product>

            if (filter.Price.HasValue && filter.Criteria.HasValue)
            {
                query = filter.Criteria switch
                {
                    PriceCriteria.GreaterThan => query.Where(p => p.Price > filter.Price.Value),
                    PriceCriteria.LessThan => query.Where(p => p.Price < filter.Price.Value),
                    PriceCriteria.EqualTo => query.Where(p => p.Price == filter.Price.Value),
                    _ => query
                };
            }

            return await query.ToPagedListAsync(filter.PageNumber, filter.PageSize);
        }
    }
}
