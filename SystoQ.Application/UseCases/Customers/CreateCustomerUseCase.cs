using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;

namespace SystoQ.Application.UseCases.Customers
{
    public class CreateCustomerUseCase
    {
        private readonly ICustumerRepository _customerRepository;

        public CreateCustomerUseCase(ICustumerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> ExecuteAsync(string name, string email, string? phoneNumber)
        {
            var customer = new Customer(name, email, phoneNumber);
            await _customerRepository.AddCustomerAsync(customer);
            return customer;
        }
    }
}
