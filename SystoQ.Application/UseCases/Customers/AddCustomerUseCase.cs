using SystoQ.Domain.Entities;
using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Customers
{
    public class AddCustomerUseCase
    {
        private readonly IUnitOfWork _uow;

        public AddCustomerUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Customer> ExecuteAsync(string name, string? email, string? phoneNumber)
        {
            var customer = new Customer(name, email, phoneNumber);
            _uow.CustomerRepository.Create(customer);
            await _uow.CommitAsync();

            return customer;
        }
    }
}
