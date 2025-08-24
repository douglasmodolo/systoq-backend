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

        public async Task<IPagedList<Product>?> GetProductsPriceFilterAsync(ProductsPriceFilter productsPriceFilter)
        {
            var products = await GetAllAsync();
            var productsQueryable = products.AsQueryable();

            if (productsPriceFilter.Price.HasValue && !string.IsNullOrEmpty(productsPriceFilter.PriceCriterias))
            {
                switch (productsPriceFilter.PriceCriterias.ToLower())
                {
                    case "greaterthan":
                        productsQueryable = productsQueryable.Where(p => p.Price > productsPriceFilter.Price.Value);
                        break;
                    case "lessthan":
                        productsQueryable = productsQueryable.Where(p => p.Price < productsPriceFilter.Price.Value);
                        break;
                    case "equalto":
                        productsQueryable = productsQueryable.Where(p => p.Price == productsPriceFilter.Price.Value);
                        break;
                    default:
                        throw new ArgumentException("Invalid price criteria specified.");
                }
            }

            var pagedProducts = await productsQueryable.ToPagedListAsync(productsPriceFilter.PageNumber, productsPriceFilter.PageSize);

            return pagedProducts;
        }
    }
}
