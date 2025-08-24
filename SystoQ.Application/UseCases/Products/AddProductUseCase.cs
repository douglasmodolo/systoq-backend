using SystoQ.Domain.Entities;
using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Products
{
    public class AddProductUseCase
    {
        private readonly IUnitOfWork _uow;

        public AddProductUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Product> ExecuteAsync(string name, string? description, decimal price, int stock)
        {
            var product = new Product(name, description, price, stock);
            
            _uow.ProductRepository.Create(product);

            await _uow.CommitAsync();

            return product;
        }
    }
}
