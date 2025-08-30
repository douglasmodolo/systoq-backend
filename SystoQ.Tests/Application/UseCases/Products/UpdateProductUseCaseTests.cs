using NSubstitute;
using System.Reflection;
using SystoQ.Application.UseCases.Products;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Products
{
    [TestFixture]
    public class UpdateProductUseCaseTests
    {
        private IUnitOfWork _uow;
        private IProductRepository _productRepository;
        private UpdateProductUseCase _updateProductUseCase;
        
        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _productRepository = Substitute.For<IProductRepository>();
            _uow.ProductRepository.Returns(_productRepository);
            _updateProductUseCase = new UpdateProductUseCase(_uow);
        }
        
        [Test]
        public async Task ExecuteAsync_ShouldUpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product("Old Name", "Old Description", 5.99m, 50);
            var idPropertyExistingProduct = typeof(Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            idPropertyExistingProduct.SetValue(existingProduct, productId);

            var updatedProduct = new Product("New Name", "New Description", 10.99m, 100);
            var idPropertyUpdatedProduct = typeof(Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            idPropertyUpdatedProduct.SetValue(updatedProduct, productId);
            
            _productRepository.Update(Arg.Any<Product>()).Returns(updatedProduct);
            
            // Act
            var result = await _updateProductUseCase.ExecuteAsync(productId, updatedProduct);
            
            // Assert
            Assert.That(result.Name, Is.EqualTo("New Name"));
            Assert.That(result.Description, Is.EqualTo("New Description"));
            Assert.That(result.Price, Is.EqualTo(10.99m));
            Assert.That(result.Stock, Is.EqualTo(100));
            _productRepository.Received(1).Update(Arg.Is<Product>(p =>
                p.Id == productId &&
                p.Name == "New Name" &&
                p.Description == "New Description" &&
                p.Price == 10.99m &&
                p.Stock == 100));
            await _uow.Received(1).CommitAsync();
        }
    }
}
