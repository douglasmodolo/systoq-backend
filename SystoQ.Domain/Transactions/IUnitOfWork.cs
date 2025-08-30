using SystoQ.Domain.Repositories;

namespace SystoQ.Domain.Transactions
{
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        IProductRepository ProductRepository { get; }
        IOrderRepository OrderRepository { get; }

        Task CommitAsync();
    }
}
