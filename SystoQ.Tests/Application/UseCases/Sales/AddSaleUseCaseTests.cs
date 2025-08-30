using NSubstitute;
using SystoQ.Application.UseCases.Sales;
using SystoQ.Application.UseCases.Sales.DTOs;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Sales
{
    [TestFixture]
    public class AddSaleUseCaseTests
    {
        private IUnitOfWork _uow;
        private ISaleRepository _saleRepository;
        private ICustomerRepository _customerRepository;
        private AddSaleUseCase _addSaleUseCase;

        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _saleRepository = Substitute.For<ISaleRepository>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _uow.SaleRepository.Returns(_saleRepository);
            _uow.CustomerRepository.Returns(_customerRepository);
            _addSaleUseCase = new AddSaleUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldAddSale()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer("Test Customer", "email@email.com", "1234567890");

            _customerRepository.GetByIdAsync(customerId).Returns(customer);

            var items = new List<SaleItemInputDto>
            {
                new SaleItemInputDto { ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 50m },
                new SaleItemInputDto { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100 },
            };

            var result = await _addSaleUseCase.ExecuteAsync(items, customerId);

            Assert.IsNotNull(result);
            Assert.That(result.CustomerId, Is.EqualTo(customer.Id));
            Assert.That(result.Items.Count, Is.EqualTo(items.Count));

            _saleRepository.Received(1).Create(Arg.Is<Sale>(s =>
                s.CustomerId == customer.Id &&
                s.Items.Count == items.Count &&
                s.Items.All(i => items.Any(dto => dto.ProductId == i.ProductId 
                        && dto.Quantity == i.Quantity 
                        && dto.UnitPrice == i.UnitPrice))
            ));

            await _uow.Received(1).CommitAsync();
        }
    }
}
