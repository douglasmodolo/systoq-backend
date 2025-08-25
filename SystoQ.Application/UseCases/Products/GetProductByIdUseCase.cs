using SystoQ.Domain.Entities;
using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Products
{
    public class GetProductByIdUseCase
    {
        private readonly IUnitOfWork _uow;
        
        public GetProductByIdUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }
        
        public async Task<Product?> ExecuteAsync(Guid id)
        {
            var product = await _uow.ProductRepository.GetByIdAsync(id);
            return product;
        }
    }
}
