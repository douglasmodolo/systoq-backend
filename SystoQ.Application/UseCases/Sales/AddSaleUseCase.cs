using SystoQ.Application.UseCases.Sales.DTOs;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Sales
{
    public class AddSaleUseCase
    {
        private readonly IUnitOfWork _uow;

        private readonly Guid DefaultCustomerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        
        public AddSaleUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Sale> ExecuteAsync(List<SaleItemInputDto> items, Guid? customerId = null)
        {
            var idCustomer = customerId ?? DefaultCustomerId;

            var customer = await _uow.CustomerRepository.GetByIdAsync(idCustomer);
            
            if (customer == null)
            {
                throw new ArgumentException("Cliente não encontrado.", nameof(customerId));
            }

            var sale = new Sale(customer.Id);

            foreach (var itemDto in items)
            {
                var saleItem = new SaleItem(sale.Id, itemDto.ProductId, itemDto.Quantity, itemDto.UnitPrice);
                sale.AddItem(saleItem);
            }

            _uow.SaleRepository.Create(sale);
            await _uow.CommitAsync();
            return sale;
        }
    }
}
