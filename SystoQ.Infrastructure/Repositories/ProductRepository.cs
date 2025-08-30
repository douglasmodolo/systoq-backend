using Microsoft.EntityFrameworkCore;
using SystoQ.Domain.Common.Enums;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Products;
using SystoQ.Domain.Repositories;
using SystoQ.Infrastructure.Persistence;
using X.PagedList;

namespace SystoQ.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(SystoQDbContext context) : base(context)
        {            
        }

        public async Task<IPagedList<Product>?> GetAllPaginatedAsync(ProductsParameters productsParams)
        {
            var products = await GetAllAsync();
            var ordenedProducts = products.OrderBy(p => p.Name);

            var pagedProducts = await ordenedProducts.ToPagedListAsync(productsParams.PageNumber, productsParams.PageSize);

            return pagedProducts;
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

        public Task<IPagedList<Product>?> SearchByNameAsync(ProductSearchFilter filter)
        {
            return _context.Products
                .Where(p => p.Name.Contains(filter.Name!))
                .ToPagedListAsync(filter.PageNumber, filter.PageSize);
        }
    }
}
