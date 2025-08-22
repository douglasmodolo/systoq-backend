using Microsoft.EntityFrameworkCore;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;
using SystoQ.Infrastructure.Persistence;

namespace SystoQ.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SystoQDbContext _context;

        public ProductRepository(SystoQDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            return _context.Products.FindAsync(id).AsTask();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
