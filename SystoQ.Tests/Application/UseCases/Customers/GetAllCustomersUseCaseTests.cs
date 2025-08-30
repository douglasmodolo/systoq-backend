using NSubstitute;
using SystoQ.Application.UseCases.Customers;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Customers
{
    [TestFixture]
    public class GetAllCustomersUseCaseTests
    {
        private IUnitOfWork _uow;
        private ICustomerRepository _customerRepository;
        private GetAllCustomersUseCase _getAllCustomersUseCase;

        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _uow.CustomerRepository.Returns(_customerRepository);
            _getAllCustomersUseCase = new GetAllCustomersUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnPagedCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer("Customer A", "emailA@email.com", "8888-8888"),
                new Customer("Customer B", "emailB@email.com", "8888-8888"),
                new Customer("Customer C", "emailC@email.com", "8888-8888"),
                new Customer("Customer D", "emailD@email.com", "8888-8888"),
            };

            _customerRepository.GetAllAsync().Returns(await Task.FromResult(customers.AsQueryable()).ConfigureAwait(false));

            var customersParams = new Domain.Filters.Customers.CustomersParameters
            {
                PageNumber = 1,
                PageSize = 2
            };

            // Act
            var result = await _getAllCustomersUseCase.ExecuteAsync(customersParams);

            // Assert
            Assert.That(result.PageNumber, Is.EqualTo(customersParams.PageNumber));
            Assert.That(result.PageSize, Is.EqualTo(customersParams.PageSize));
            Assert.That(result.TotalItemCount, Is.EqualTo(customers.Count));
            Assert.That(result.Count, Is.EqualTo(2)); // Should return only 2 items for page size of 2
            Assert.That(result[0].Name, Is.EqualTo("Customer A"));
            Assert.That(result[1].Name, Is.EqualTo("Customer B"));
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnEmptyPagedCustomers_WhenNoCustomersExist()
        {
            // Arrange
            var customers = new List<Customer>();
            _customerRepository.GetAllAsync().Returns(await Task.FromResult(customers.AsQueryable()).ConfigureAwait(false));
            var customersParams = new Domain.Filters.Customers.CustomersParameters
            {
                PageNumber = 1,
                PageSize = 2
            };
            
            // Act
            var result = await _getAllCustomersUseCase.ExecuteAsync(customersParams);
            
            // Assert
            Assert.That(result.PageNumber, Is.EqualTo(customersParams.PageNumber));
            Assert.That(result.PageSize, Is.EqualTo(customersParams.PageSize));
            Assert.That(result.TotalItemCount, Is.EqualTo(0));
            Assert.That(result.Count, Is.EqualTo(0)); // Should return 0 items as there are no customers
        }
    }
}
