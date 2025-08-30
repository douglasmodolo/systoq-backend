using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Products;
using SystoQ.Domain.Transactions;
using X.PagedList;

namespace SystoQ.Application.UseCases.Products
{
    public class SearchProductsByNameUseCase
    {
        private readonly IUnitOfWork _uow;

        public SearchProductsByNameUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IPagedList<Product>> ExecuteAsync(ProductSearchFilter filter)
        {
            return await _uow.ProductRepository.SearchByNameAsync(filter);
        }
    }
}
