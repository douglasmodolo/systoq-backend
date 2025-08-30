using NSubstitute;
using SystoQ.Application.UseCases.Orders;
using SystoQ.Application.UseCases.Orders.DTOs;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Orders
{
    [TestFixture]
    public class AddOrderUseCaseTests
    {
        private IUnitOfWork _uow;
        private IOrderRepository _orderRepository;
        private ICustomerRepository _customerRepository;
        private AddOrderUseCase _addOrderUseCase;

        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _orderRepository = Substitute.For<IOrderRepository>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _uow.OrderRepository.Returns(_orderRepository);
            _uow.CustomerRepository.Returns(_customerRepository);
            _addOrderUseCase = new AddOrderUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldAddOrder()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer("Test Customer", "email@email.com", "1234567890");

            _customerRepository.GetByIdAsync(customerId).Returns(customer);

            var items = new List<OrderItemInputDto>
            {
                new OrderItemInputDto { ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 50m },
                new OrderItemInputDto { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100 },
            };

            var result = await _addOrderUseCase.ExecuteAsync(items, customerId);

            Assert.IsNotNull(result);
            Assert.That(result.CustomerId, Is.EqualTo(customer.Id));
            Assert.That(result.Items.Count, Is.EqualTo(items.Count));

            _orderRepository.Received(1).Create(Arg.Is<Order>(s =>
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
