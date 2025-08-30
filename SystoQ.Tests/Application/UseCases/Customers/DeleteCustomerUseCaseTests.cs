using NSubstitute;
using SystoQ.Application.UseCases.Customers;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Customers
{
    [TestFixture]
    public class DeleteCustomerUseCaseTests
    {
        private IUnitOfWork _uow;
        private ICustomerRepository _customerRepository;
        private DeleteCustomerUseCase _deleteCustomerUseCase;
        
        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _uow.CustomerRepository.Returns(_customerRepository);
            _deleteCustomerUseCase = new DeleteCustomerUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldDeleteExistingCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer("Jhon Doe", "email@email.com", "8888-8888");

            _customerRepository.GetByIdAsync(customerId).Returns(customer);

            // Act
            var result = await _deleteCustomerUseCase.ExecuteAsync(customerId);

            // Assert
            Assert.That(result, Is.True);
            _customerRepository.Received(1).Delete(customerId);
            await _uow.Received(1).CommitAsync();
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenCustomerDoesNotExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _customerRepository.GetByIdAsync(customerId).Returns((Customer?)null);
            
            // Act
            var result = await _deleteCustomerUseCase.ExecuteAsync(customerId);
            
            // Assert
            Assert.That(result, Is.False);
            _customerRepository.DidNotReceive().Delete(Arg.Any<Guid>());
            await _uow.DidNotReceive().CommitAsync();
        }
    }
}
