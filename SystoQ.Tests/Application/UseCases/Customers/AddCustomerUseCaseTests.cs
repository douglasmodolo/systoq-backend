using NSubstitute;
using SystoQ.Application.UseCases.Customers;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;

namespace SystoQ.Tests.Application.UseCases.Customers
{
    [TestFixture]
    public class AddCustomerUseCaseTests
    {
        private ICustomerRepository _customerRepository;
        private AddCustomerUseCase _addCustomerUseCase;
        
        [SetUp]
        public void SetUp()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _addCustomerUseCase = new AddCustomerUseCase(_customerRepository);
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

            await _customerRepository.Received(1).AddCustomerAsync(Arg.Is<Customer>(c =>
                c.Name == name &&
                c.Email == email &&
                c.PhoneNumber == phoneNumber));
        }

    }
}
