using NSubstitute;
using SystoQ.Application.UseCases.Products;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Products
{
    [TestFixture]
    public class AddProductUseCaseTests
    {
        private IUnitOfWork _uow;
        private IProductRepository _productRepository;
        private AddProductUseCase _addProductUseCase;

        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _productRepository = Substitute.For<IProductRepository>();
            _uow.ProductRepository.Returns(_productRepository);
            _addProductUseCase = new AddProductUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldAddProduct()
        {
            // Arrange
            var name = "Test Product";
            var description = "This is a test product.";
            var price = 9.99m;
            var stock = 100;

            // Act
            var result = await _addProductUseCase.ExecuteAsync(name, description, price, stock);

            // Assert
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.Description, Is.EqualTo(description));
            Assert.That(result.Price, Is.EqualTo(price));
            Assert.That(result.Stock, Is.EqualTo(stock));

            _productRepository.Received(1).Create(Arg.Is<Product>(p =>
                p.Name == name &&
                p.Description == description &&
                p.Price == price &&
                p.Stock == stock));

            await _uow.Received(1).CommitAsync();
        }
    }
}
