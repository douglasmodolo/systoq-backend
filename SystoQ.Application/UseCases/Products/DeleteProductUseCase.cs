using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Products
{
    public class DeleteProductUseCase
    {
        private readonly IUnitOfWork _uow;

        public DeleteProductUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> ExecuteAsync(Guid productId)
        {
            var existingProduct = await _uow.ProductRepository.GetByIdAsync(productId);
            if (existingProduct == null)
            {
                return false;
            }

            _uow.ProductRepository.Delete(productId);

            await _uow.CommitAsync();
            
            return true;
        }
    }
}
