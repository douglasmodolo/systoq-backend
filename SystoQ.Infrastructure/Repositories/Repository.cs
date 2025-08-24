using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SystoQ.Domain.Repositories;
using SystoQ.Infrastructure.Persistence;

namespace SystoQ.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly SystoQDbContext _context;

        public Repository(SystoQDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public bool Delete(Guid id)
        {
            var entity = _context.Set<T>().Find(id);

            if (entity == null)
                return false;

            _context.Set<T>().Remove(entity);

            return _context.SaveChanges() > 0;
        }
    }
}
