using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;

namespace SystoQ.Application.UseCases.Customers
{
    public class AddCustomerUseCase
    {
        private readonly ICustumerRepository _customerRepository;

        public AddCustomerUseCase(ICustumerRepository customerRepository)
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
