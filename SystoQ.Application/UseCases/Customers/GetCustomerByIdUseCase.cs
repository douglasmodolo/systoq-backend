using SystoQ.Domain.Entities;
using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Customers
{
    public class GetCustomerByIdUseCase
    {
        private readonly IUnitOfWork _uow;

        public GetCustomerByIdUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Customer?> ExecuteAsync(Guid id)
        {
            var customer = await _uow.CustomerRepository.GetByIdAsync(id);
            return customer;
        }
    }
}
