using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Customers
{
    public class DeleteCustomerUseCase
    {
        private readonly IUnitOfWork _uow;
        
        public DeleteCustomerUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }
        
        public async Task<bool> ExecuteAsync(Guid customerId)
        {
            var existingCustomer = await _uow.CustomerRepository.GetByIdAsync(customerId);
         
            if (existingCustomer == null)
            {
                return false;
            }
            
            _uow.CustomerRepository.Delete(customerId);
            await _uow.CommitAsync();
            
            return true;
        }
    }
}
