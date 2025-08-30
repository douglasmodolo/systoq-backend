using SystoQ.Domain.Entities;
using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Products
{
    public class UpdateProductUseCase
    {
        private readonly IUnitOfWork _uow;

        public UpdateProductUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Product> ExecuteAsync(Guid id, Product product)
        {
            var updatedProduct = _uow.ProductRepository.Update(product);
            await _uow.CommitAsync();

            return updatedProduct;
        }
    }
}
