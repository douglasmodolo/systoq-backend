using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Products;
using SystoQ.Domain.Transactions;
using X.PagedList;

namespace SystoQ.Application.UseCases.Products
{
    public class GetProductsByPriceRangeUseCase
    {
        private readonly IUnitOfWork _uow;
        
        public GetProductsByPriceRangeUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }
        
        public async Task<IPagedList<Product>> ExecuteAsync(ProductsPriceFilter filter)
        {
            if (filter.Price is null || filter.Criteria is null)
                throw new ArgumentException("Preço e critério devem ser fornecidos.");

            var products = await _uow.ProductRepository.GetProductsPriceFilterAsync(filter);

            if (products is null || !products.Any())
                return new PagedList<Product>(new List<Product>(), filter.PageNumber, filter.PageSize);

            return products;
        }
    }
}
