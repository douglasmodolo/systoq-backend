using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Customers;
using SystoQ.Domain.Transactions;
using X.PagedList;

namespace SystoQ.Application.UseCases.Customers
{
    public class GetAllCustomersUseCase
    {
        private readonly IUnitOfWork _uow;
 
        public GetAllCustomersUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }
 
        public async Task<IPagedList<Customer>> ExecuteAsync(CustomersParameters customersParams)
        {
            var customers = await _uow.CustomerRepository.GetAllAsync();
            var ordenedCustomers = customers.OrderBy(c => c.Name);
 
            var pagedCustomers = await ordenedCustomers.ToPagedListAsync(customersParams.PageNumber, customersParams.PageSize);
            
            return pagedCustomers;
        }
    }
}
