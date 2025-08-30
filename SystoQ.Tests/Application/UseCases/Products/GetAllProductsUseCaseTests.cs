using NSubstitute;
using SystoQ.Application.UseCases.Products;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Products;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Products
{
    [TestFixture]
    public class GetAllProductsUseCaseTests
    {
        private IUnitOfWork _uow;
        private IProductRepository _productRepository;
        private GetAllProductsUseCase _getAllProductsUseCase;
        
        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _productRepository = Substitute.For<IProductRepository>();
            _uow.ProductRepository.Returns(_productRepository);
            _getAllProductsUseCase = new GetAllProductsUseCase(_uow);
        }
        
        [Test]
        public async Task ExecuteAsync_ShouldReturnPagedProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product("Product A", "Description A", 10.0m, 100),
                new Product("Product B", "Description B", 20.0m, 200),
                new Product("Product C", "Description C", 30.0m, 300),
                new Product("Product D", "Description D", 40.0m, 400),
                new Product("Product E", "Description E", 50.0m, 500)
            };
            _productRepository.GetAllAsync().Returns(await Task.FromResult(products.AsQueryable()).ConfigureAwait(false));
            var productsParams = new ProductsParameters
            {
                PageNumber = 1,
                PageSize = 2
            };
            // Act
            var result = await _getAllProductsUseCase.ExecuteAsync(productsParams);
            // Assert
            Assert.That(result.PageNumber, Is.EqualTo(productsParams.PageNumber));
            Assert.That(result.PageSize, Is.EqualTo(productsParams.PageSize));
            Assert.That(result.TotalItemCount, Is.EqualTo(products.Count));
            Assert.That(result.Count, Is.EqualTo(2)); // Should return only 2 items for page size of 2
            Assert.That(result[0].Name, Is.EqualTo("Product A"));
            Assert.That(result[1].Name, Is.EqualTo("Product B"));
        }
    }
}
