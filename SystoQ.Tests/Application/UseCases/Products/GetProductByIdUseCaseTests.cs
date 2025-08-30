using NSubstitute;
using System.Reflection;
using SystoQ.Application.UseCases.Products;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Products
{
    [TestFixture]
    public class GetProductByIdUseCaseTests
    {
        private IUnitOfWork _uow;
        private IProductRepository _productRepository;
        private GetProductByIdUseCase _getProductByIdUseCase;

        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _productRepository = Substitute.For<IProductRepository>();
            _uow.ProductRepository.Returns(_productRepository);
            _getProductByIdUseCase = new GetProductByIdUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Test Product", "This is a test product.", 9.99m, 100);

            // reflection
            var idProperty = typeof(Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            idProperty.SetValue(product, productId);

            _productRepository.GetByIdAsync(productId).Returns(Task.FromResult<Product?>(product));
            // Act
            var result = await _getProductByIdUseCase.ExecuteAsync(productId);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(productId));
            Assert.That(result?.Name, Is.EqualTo("Test Product"));
            Assert.That(result?.Description, Is.EqualTo("This is a test product."));
            Assert.That(result?.Price, Is.EqualTo(9.99m));
            Assert.That(result?.Stock, Is.EqualTo(100));
            await _productRepository.Received(1).GetByIdAsync(productId);
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productRepository.GetByIdAsync(productId).Returns(Task.FromResult<Product?>(null));
            // Act
            var result = await _getProductByIdUseCase.ExecuteAsync(productId);
            // Assert
            Assert.That(result, Is.Null);
            await _productRepository.Received(1).GetByIdAsync(productId);
        }
    }
}
