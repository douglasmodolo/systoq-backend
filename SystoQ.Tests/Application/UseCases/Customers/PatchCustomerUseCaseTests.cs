using NSubstitute;
using System.Linq.Expressions;
using SystoQ.Application.UseCases.Customers;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Customers
{
    [TestFixture]
    public class PatchCustomerUseCaseTests
    {
        private IUnitOfWork _uow;
        private ICustomerRepository _customerRepository;
        private PatchCustomerUseCase _patchCustomerUseCase;

        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _uow.CustomerRepository.Returns(_customerRepository);
            _patchCustomerUseCase = new PatchCustomerUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldPatchCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var existingCustomer = new Customer("Jhon Doe", "email@email.com", "8888-8888");

            // reflection
            var idProperty = typeof(Customer).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            idProperty.SetValue(existingCustomer, customerId);
            _customerRepository.GetAsync(Arg.Any<Expression<Func<Customer, bool>>>())
                .Returns(Task.FromResult(existingCustomer));

            var newName = "Jane Doe";
            var newEmail = "newemail@email.com";
            var newPhoneNumber = "9999-9999";

            // Act
            var result = await _patchCustomerUseCase.ExecuteAsync(customerId, newName, newEmail, newPhoneNumber);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Name, Is.EqualTo(newName));
            Assert.That(result.Email, Is.EqualTo(newEmail));
            Assert.That(result.PhoneNumber, Is.EqualTo(newPhoneNumber));
            _customerRepository.Received(1).Update(Arg.Is<Customer>(c =>
                c.Id == customerId &&
                c.Name == newName &&
                c.Email == newEmail &&
                c.PhoneNumber == newPhoneNumber));
            await _uow.Received(1).CommitAsync();
        }

        [Test]
        public async Task ExecuteAsync_ShouldReturnNull_WhenCustomerNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            _customerRepository.GetAsync(c => c.Id == customerId)
                .Returns(Task.FromResult<Customer?>(null));

            // Act
            var result = await _patchCustomerUseCase.ExecuteAsync(customerId, "New Name", "newemail@email.com", "9999-9999");

            // Assert
            Assert.That(result, Is.Null);
            await _uow.DidNotReceive().CommitAsync();
        }
    }
}
