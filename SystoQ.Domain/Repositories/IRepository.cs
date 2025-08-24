using System.Linq.Expressions;

namespace SystoQ.Domain.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(Guid id);
        T Create(T entity);
        T Update(T entity);
        bool Delete(Guid id);
    }
}
