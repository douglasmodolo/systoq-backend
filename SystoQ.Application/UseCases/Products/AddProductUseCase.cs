using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;

namespace SystoQ.Application.UseCases.Products
{
    public class AddProductUseCase
    {
        private readonly IProductRepository _productRepository;
        
        public AddProductUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        public async Task<Product> ExecuteAsync(string name, string? description, decimal price, int stock)
        {
            var product = new Product(name, description, price, stock);
            
            await _productRepository.AddProductAsync(product);
            
            return product;
        }
    }
}
