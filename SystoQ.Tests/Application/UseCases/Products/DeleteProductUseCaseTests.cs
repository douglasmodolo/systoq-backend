using NSubstitute;
using System.Reflection;
using SystoQ.Application.UseCases.Products;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Products
{
    [TestFixture]
    public class DeleteProductUseCaseTests
    {
        private IUnitOfWork _uow;
        private IProductRepository _productRepository;
        private DeleteProductUseCase _deleteProductUseCase;
        
        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _productRepository = Substitute.For<IProductRepository>();
            _uow.ProductRepository.Returns(_productRepository);
            _deleteProductUseCase = new DeleteProductUseCase(_uow);
        }
        
        [Test]
        public async Task ExecuteAsync_ShouldDeleteExistingProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Test Product", "This is a test product.", 9.99m, 100);

            // reflection
            //var idProperty = typeof(Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            //idProperty.SetValue(product, productId);

            _productRepository.GetByIdAsync(productId).Returns(product);
            
            // Act
            var result = await _deleteProductUseCase.ExecuteAsync(productId);
            
            // Assert
            Assert.That(result, Is.True);
            _productRepository.Received(1).Delete(productId);
            await _uow.Received(1).CommitAsync();
        }
        
        [Test]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productRepository.GetByIdAsync(productId).Returns((Product?)null);
            
            // Act
            var result = await _deleteProductUseCase.ExecuteAsync(productId);
            
            // Assert
            Assert.That(result, Is.False);
            _productRepository.DidNotReceive().Delete(Arg.Any<Guid>());
            await _uow.DidNotReceive().CommitAsync();
        }
    }
}
