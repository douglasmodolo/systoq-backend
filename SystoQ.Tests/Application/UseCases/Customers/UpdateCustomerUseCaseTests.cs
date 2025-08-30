using NSubstitute;
using SystoQ.Application.UseCases.Customers;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Customers
{
    [TestFixture]
    public class UpdateCustomerUseCaseTests
    {
        private IUnitOfWork _uow;
        private ICustomerRepository _customerRepository;
        private UpdateCustomerUseCase _updateCustomerUseCase;

        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _uow.CustomerRepository.Returns(_customerRepository);
            _updateCustomerUseCase = new UpdateCustomerUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldUpdateCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer("Old Name", "old@email.com", "8888-8888");
            var idPropertyExistingCustomer = typeof(Customer).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropertyExistingCustomer.SetValue(existingCustomer, customerId);

            var updatedCustomer = new Customer("New Name", "new@email.com", "9999-9999");
            var idPropertyUpdatedCustomer = typeof(Customer).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idPropertyUpdatedCustomer.SetValue(updatedCustomer, customerId);

            _customerRepository.Update(Arg.Any<Customer>()).Returns(updatedCustomer);

            // Act
            var result = await _updateCustomerUseCase.ExecuteAsync(customerId, updatedCustomer);

            // Assert
            Assert.That(result.Name, Is.EqualTo("New Name"));
            Assert.That(result.Email, Is.EqualTo("new@email.com"));
            Assert.That(result.PhoneNumber, Is.EqualTo("9999-9999"));
            _customerRepository.Received(1).Update(Arg.Is<Customer>(c =>
                c.Id == customerId &&
                c.Name == "New Name" &&
                c.Email == "new@email.com" &&
                c.PhoneNumber == "9999-9999"));
            await _uow.Received(1).CommitAsync();
        }
    }
}
