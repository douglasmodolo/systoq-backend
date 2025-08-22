using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;

namespace SystoQ.Application.UseCases.Products
{
    public class CreateProductUseCase
    {
        private readonly IProductRepository _productRepository;
        
        public CreateProductUseCase(IProductRepository productRepository)
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
