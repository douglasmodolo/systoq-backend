using NSubstitute;
using SystoQ.Application.UseCases.Sales;
using SystoQ.Application.UseCases.Sales.DTOs;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;

namespace SystoQ.Tests.Application.UseCases.Sales
{
    [TestFixture]
    public class AddSaleUseCaseTests
    {
        private ISaleRepository _saleRepository;
        private ICustomerRepository _customerRepository;
        private AddSaleUseCase _addSaleUseCase;

        [SetUp]
        public void SetUp()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _addSaleUseCase = new AddSaleUseCase(_saleRepository, _customerRepository);
        }

        [Test]
        public async Task ExecuteAsync_ShouldAddSale()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer("Test Customer", "email@email.com", "1234567890");

            _customerRepository.GetCustomerByIdAsync(customerId).Returns(customer);

            var items = new List<SaleItemInputDto>
            {
                new SaleItemInputDto { ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 50m },
                new SaleItemInputDto { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100 },
            };

            var result = await _addSaleUseCase.ExecuteAsync(items, customerId);

            Assert.IsNotNull(result);
            Assert.That(result.CustomerId, Is.EqualTo(customer.Id));
            Assert.That(result.Items.Count, Is.EqualTo(items.Count));

            await _saleRepository.Received(1).AddSaleAsync(Arg.Is<Sale>(s =>
                s.CustomerId == customer.Id &&
                s.Items.Count == items.Count &&
                s.Items.All(i => items.Any(dto => dto.ProductId == i.ProductId 
                        && dto.Quantity == i.Quantity 
                        && dto.UnitPrice == i.UnitPrice))
            ));
        }
    }
}
