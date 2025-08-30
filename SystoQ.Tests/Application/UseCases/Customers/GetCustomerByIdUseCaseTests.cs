using NSubstitute;
using SystoQ.Application.UseCases.Customers;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Customers
{
    [TestFixture]
    public class GetCustomerByIdUseCaseTests
    {
        private IUnitOfWork _uow;
        private ICustomerRepository _customerRepository;
        private GetCustomerByIdUseCase _getCustomerByIdUseCase;
        
        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _uow.CustomerRepository.Returns(_customerRepository);
            _getCustomerByIdUseCase = new GetCustomerByIdUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer("Jhon Doe", "email@email.com", "8888-8888");

            _customerRepository.GetByIdAsync(customerId).Returns(Task.FromResult<Customer?>(customer));

            // Act
            var result = await _getCustomerByIdUseCase.ExecuteAsync(customerId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(customer.Id));
            Assert.That(result?.Name, Is.EqualTo("Jhon Doe"));
            Assert.That(result?.Email, Is.EqualTo("email@email.com"));
            Assert.That(result?.PhoneNumber, Is.EqualTo("8888-8888"));
            await _customerRepository.Received(1).GetByIdAsync(customerId);
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _customerRepository.GetByIdAsync(customerId).Returns(Task.FromResult<Customer?>(null));
            
            // Act
            var result = await _getCustomerByIdUseCase.ExecuteAsync(customerId);
         
            // Assert
            Assert.That(result, Is.Null);
            await _customerRepository.Received(1).GetByIdAsync(customerId);
        }
    }
}
