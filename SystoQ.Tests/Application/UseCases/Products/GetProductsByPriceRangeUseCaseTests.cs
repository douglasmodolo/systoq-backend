using NSubstitute;
using SystoQ.Application.UseCases.Products;
using SystoQ.Domain.Common.Enums;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Products;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;
using X.PagedList;

namespace SystoQ.Tests.Application.UseCases.Products
{
    [TestFixture]
    public class GetProductsByPriceRangeUseCaseTests
    {
        private IUnitOfWork _uow;
        private IProductRepository _productRepository;
        private GetProductsByPriceRangeUseCase _getProductsByPriceRangeUseCase;

        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _productRepository = Substitute.For<IProductRepository>();
            _uow.ProductRepository.Returns(_productRepository);
            _getProductsByPriceRangeUseCase = new GetProductsByPriceRangeUseCase(_uow);
        }
        
        [Test]
        public async Task ExecuteAsync_ShouldReturnProductsInPriceRange()
        {
            // Arrange
            var filter = new ProductsPriceFilter
            {
                Price = 50.0m,
                Criteria = PriceCriteria.LessThan,
                PageNumber = 1,
                PageSize = 10
            };
            var products = new List<Product>
            {
                new Product("Product 1", "Description 1", 30.0m, 100),
                new Product("Product 2", "Description 2", 40.0m, 200)
            }.ToPagedList(filter.PageNumber, filter.PageSize);
            _productRepository.GetProductsPriceFilterAsync(filter).Returns(Task.FromResult(products));
            
            // Act
            var result = await _getProductsByPriceRangeUseCase.ExecuteAsync(filter);
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Price, Is.LessThan(filter.Price));
            Assert.That(result[1].Price, Is.LessThan(filter.Price));
            await _productRepository.Received(1).GetProductsPriceFilterAsync(filter);
        }

        [Test]
        public void ExecuteAsync_ShouldThrowArgumentException_WhenPriceOrCriteriaIsNull()
        {
            // Arrange
            var filterWithNullPrice = new ProductsPriceFilter
            {
                Price = null,
                Criteria = PriceCriteria.LessThan,
                PageNumber = 1,
                PageSize = 10
            };
            var filterWithNullCriteria = new ProductsPriceFilter
            {
                Price = 50.0m,
                Criteria = null,
                PageNumber = 1,
                PageSize = 10
            };
            // Act & Assert
            var ex1 = Assert.ThrowsAsync<ArgumentException>(async () => await _getProductsByPriceRangeUseCase.ExecuteAsync(filterWithNullPrice));
            Assert.That(ex1.Message, Is.EqualTo("Preço e critério devem ser fornecidos."));
            var ex2 = Assert.ThrowsAsync<ArgumentException>(async () => await _getProductsByPriceRangeUseCase.ExecuteAsync(filterWithNullCriteria));
            Assert.That(ex2.Message, Is.EqualTo("Preço e critério devem ser fornecidos."));
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnEmptyPagedList_WhenNoProductsFound()
        {
            // Arrange
            var filter = new ProductsPriceFilter
            {
                Price = 10.0m,
                Criteria = PriceCriteria.LessThan,
                PageNumber = 1,
                PageSize = 10
            };
            _productRepository.GetProductsPriceFilterAsync(filter).Returns(Task.FromResult((IPagedList<Product>)null));
            // Act
            var result = await _getProductsByPriceRangeUseCase.ExecuteAsync(filter);
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
            await _productRepository.Received(1).GetProductsPriceFilterAsync(filter);
        }
    }
}
