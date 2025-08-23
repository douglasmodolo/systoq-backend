using SystoQ.Domain.Entities;

namespace SystoQ.Domain.Repositories
{
    public interface ISaleRepository
    {
        Task<Sale> AddSaleAsync(Sale sale);
    }
}
