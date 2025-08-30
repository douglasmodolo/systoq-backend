using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Products;
using X.PagedList;

namespace SystoQ.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IPagedList<Product>?> GetAllPaginatedAsync(ProductsParameters productsParams);
        Task<IPagedList<Product>?> GetProductsPriceFilterAsync(ProductsPriceFilter productsPriceFilter);
        Task<IPagedList<Product>?> SearchByNameAsync(ProductSearchFilter filter);
    }
}
