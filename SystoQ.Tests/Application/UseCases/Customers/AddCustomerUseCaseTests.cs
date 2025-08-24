using NSubstitute;
using SystoQ.Application.UseCases.Customers;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;

namespace SystoQ.Tests.Application.UseCases.Customers
{
    [TestFixture]
    public class AddCustomerUseCaseTests
    {
        private IUnitOfWork _uow;
        private ICustomerRepository _customerRepository;
        private AddCustomerUseCase _addCustomerUseCase;
        
        [SetUp]
        public void SetUp()
        {
            _uow = Substitute.For<IUnitOfWork>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _uow.CustomerRepository.Returns(_customerRepository);
            _addCustomerUseCase = new AddCustomerUseCase(_uow);
        }

        [Test]
        public async Task ExecuteAsync_ShouldAddCustomer()
        {
            //Arrange
            var name = "Test Customer";
            var email = "email@email.com";
            var phoneNumber = "1234567890";

            //Act
            var result = await _addCustomerUseCase.ExecuteAsync(name, email, phoneNumber);

            //Assert
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.PhoneNumber, Is.EqualTo(phoneNumber));

            _customerRepository.Received(1).Create(Arg.Is<Customer>(c =>
                c.Name == name &&
                c.Email == email &&
                c.PhoneNumber == phoneNumber));

            await _uow.Received(1).CommitAsync();
        }
    }
}
