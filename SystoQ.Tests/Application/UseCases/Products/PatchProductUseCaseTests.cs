using NSubstitute;
using System.Linq.Expressions;
using System.Reflection;
using SystoQ.Application.UseCases.Products;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Products
{
    [TestFixture]
    public class PatchProductUseCaseTests
    {
        private IUnitOfWork _uow;
        private IProductRepository _productRepository;
        private PatchProductUseCase _patchProductUseCase;
        
        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _productRepository = Substitute.For<IProductRepository>();
            _uow.ProductRepository.Returns(_productRepository);
            _patchProductUseCase = new PatchProductUseCase(_uow);
        }
        
        [Test]
        public async Task ExecuteAsync_ShouldPatchProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product("Test Product", "This is a test product.", 9.99m, 100);

            // reflection
            var idProperty = typeof(Product).GetProperty("Id", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            idProperty.SetValue(existingProduct, productId);
            _productRepository.GetAsync(Arg.Any<Expression<Func<Product, bool>>>())
                .Returns(Task.FromResult(existingProduct));
            var newName = "New Name";
            var newDescription = "New Description";
            var newPrice = 10.00m;
            var newStock = 100;
            
            // Act
            var result = await _patchProductUseCase.ExecuteAsync(productId, newName, newDescription, newPrice, newStock);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo(newName));
            Assert.That(result.Description, Is.EqualTo(newDescription));
            Assert.That(result.Price, Is.EqualTo(newPrice));
            Assert.That(result.Stock, Is.EqualTo(newStock));
            _productRepository.Received(1).Update(Arg.Is<Product>(p =>
                p.Id == productId &&
                p.Name == newName &&
                p.Description == newDescription &&
                p.Price == newPrice &&
                p.Stock == newStock));
            await _uow.Received(1).CommitAsync();
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnNull_WhenProductNotFound()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productRepository.GetAsync(Arg.Any<Expression<Func<Product, bool>>>())
                .Returns(Task.FromResult<Product?>(null));
            
            // Act
            var result = await _patchProductUseCase.ExecuteAsync(productId, "New Name", "New Description", 10.00m, 100);
            
            // Assert
            Assert.That(result, Is.Null);
            await _uow.DidNotReceive().CommitAsync();
        }
    }
}
