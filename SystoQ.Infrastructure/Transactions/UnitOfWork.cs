using SystoQ.Domain.Repositories;
using SystoQ.Domain.Transactions;
using SystoQ.Infrastructure.Persistence;
using SystoQ.Infrastructure.Repositories;

namespace SystoQ.Infrastructure.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private ICustomerRepository? _customerRepository;
        private IProductRepository? _productRepository;
        private IOrderRepository? _orderRepository;

        public SystoQDbContext _context { get; }

        public UnitOfWork(SystoQDbContext context) => _context = context;

        public ICustomerRepository CustomerRepository
        {
            get
            {
                return _customerRepository ??= new CustomerRepository(_context);
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository(_context);
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                return _orderRepository ??= new OrderRepository(_context);
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
