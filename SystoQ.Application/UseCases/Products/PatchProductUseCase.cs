using SystoQ.Domain.Entities;
using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Products
{
    public class PatchProductUseCase
    {
        private readonly IUnitOfWork _uow;

        public PatchProductUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Product?> ExecuteAsync(Guid id, string? name, string? description, decimal? price, int? stock)
        {
            var product = await _uow.ProductRepository.GetAsync(p => p.Id == id);
            if (product == null) return null;

            if (!string.IsNullOrEmpty(name)) product.UpdateName(name);
            if (description != null) product.UpdateDescription(description);
            if (price.HasValue) product.UpdatePrice(price.Value);
            if (stock.HasValue) product.UpdateStock(stock.Value);

            _uow.ProductRepository.Update(product);
            await _uow.CommitAsync();

            return product;
        }
    }
}
