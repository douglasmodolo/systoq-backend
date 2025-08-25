using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Products;
using SystoQ.Domain.Transactions;
using X.PagedList;

namespace SystoQ.Application.UseCases.Products
{
    public class GetAllProductsUseCase
    {
        private readonly IUnitOfWork _uow;

        public GetAllProductsUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IPagedList<Product>> ExecuteAsync(ProductsParameters productsParams)
        {
            var products = await _uow.ProductRepository.GetAllAsync();
            var ordenedProducts = products.OrderBy(p => p.Name);

            var pagedProducts = await ordenedProducts.ToPagedListAsync(productsParams.PageNumber, productsParams.PageSize);
            
            return pagedProducts;
        }
    }
}
