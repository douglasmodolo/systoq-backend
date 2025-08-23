using SystoQ.Application.UseCases.Sales.DTOs;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Repositories;

namespace SystoQ.Application.UseCases.Sales
{
    public class AddSaleUseCase
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ICustumerRepository _customerRepository;

        private readonly Guid DefaultCustomerId = Guid.Parse("00000000-0000-0000-0000-000000000001");

        public AddSaleUseCase(ISaleRepository saleRepository, ICustumerRepository customerRepository)
        {
            _saleRepository = saleRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Sale> ExecuteAsync(List<SaleItemInputDto> items, Guid? customerId = null)
        {
            var idCustomer = customerId ?? DefaultCustomerId;

            var customer = await _customerRepository.GetCustomerByIdAsync(idCustomer);
            
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

            await _saleRepository.AddSaleAsync(sale);
            return sale;
        }
    }
}
