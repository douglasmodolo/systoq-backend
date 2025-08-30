using NSubstitute;
using SystoQ.Application.UseCases.Products;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Products;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;
using X.PagedList;

namespace SystoQ.Tests.Application.UseCases.Products
{
    [TestFixture]
    public class SearchProductsByNameUseCaseTests
    {
        private IUnitOfWork _uow;
        private IProductRepository _productRepository;
        private SearchProductsByNameUseCase _searchProductsByNameUseCase;

        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _productRepository = Substitute.For<IProductRepository>();
            _uow.ProductRepository.Returns(_productRepository);
            _searchProductsByNameUseCase = new SearchProductsByNameUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnProductsMatchingName()
        {
            // Arrange
            var filter = new ProductSearchFilter
            {
                Name = "Test",
                PageNumber = 1,
                PageSize = 10
            };

            var products = new List<Product>
            {
                new Product("Test Product 1", "Description 1", 30.0m, 100),
                new Product("Another Test Product", "Description 2", 40.0m, 200)
            }.ToPagedList(filter.PageNumber, filter.PageSize);

            _productRepository.SearchByNameAsync(filter).Returns(Task.FromResult(products));

            // Act
            var result = await _searchProductsByNameUseCase.ExecuteAsync(filter);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name.ToUpper(), Does.Contain(filter.Name.ToUpper()));
            Assert.That(result[1].Name.ToUpper(), Does.Contain(filter.Name.ToUpper()));
            await _productRepository.Received(1).SearchByNameAsync(filter);
        }

        [Test]
        public void ExecuteAsync_ShouldReturnEmptyList_WhenNoProductsMatchName()
        {
            // Arrange
            var filter = new ProductSearchFilter
            {
                Name = "NonExistentProduct",
                PageNumber = 1,
                PageSize = 10
            };
            var emptyProducts = new List<Product>().ToPagedList(filter.PageNumber, filter.PageSize);
            _productRepository.SearchByNameAsync(filter).Returns(Task.FromResult(emptyProducts));
            
            // Act
            var result = _searchProductsByNameUseCase.ExecuteAsync(filter).Result;
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
            _productRepository.Received(1).SearchByNameAsync(filter);
        } 
    }
}
