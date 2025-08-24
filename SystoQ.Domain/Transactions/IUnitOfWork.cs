using SystoQ.Domain.Repositories;

namespace SystoQ.Domain.Transactions
{
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        IProductRepository ProductRepository { get; }
        ISaleRepository SaleRepository { get; }

        Task CommitAsync();
    }
}
