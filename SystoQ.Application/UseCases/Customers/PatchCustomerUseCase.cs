using SystoQ.Domain.Entities;
using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Customers
{
    public class PatchCustomerUseCase
    {
        private readonly IUnitOfWork _uow;
        
        public PatchCustomerUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }
        
        public async Task<Customer?> ExecuteAsync(Guid id, string? name, string? email, string? phoneNumber)
        {
            var customer = await _uow.CustomerRepository.GetAsync(c => c.Id == id);
            if (customer == null) return null;
            
            if (!string.IsNullOrEmpty(name)) customer.UpdateName(name);
            if (email != null) customer.UpdateEmail(email);
            if (phoneNumber != null) customer.UpdatePhoneNumber(phoneNumber);
            
            _uow.CustomerRepository.Update(customer);
            await _uow.CommitAsync();
            
            return customer;
        }
    }
}
