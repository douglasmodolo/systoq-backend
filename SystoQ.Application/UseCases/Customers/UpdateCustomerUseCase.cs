using SystoQ.Domain.Entities;
using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Customers
{
    public class UpdateCustomerUseCase
    {
        private readonly IUnitOfWork _uow;
        
        public UpdateCustomerUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }
        
        public async Task<Customer> ExecuteAsync(Guid id, Customer customer)
        {
            var updatedCustomer = _uow.CustomerRepository.Update(customer);
            await _uow.CommitAsync();
            
            return updatedCustomer;
        }
    }
}
