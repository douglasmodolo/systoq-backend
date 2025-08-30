using SystoQ.Application.UseCases.Orders.DTOs;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Transactions;

namespace SystoQ.Application.UseCases.Orders
{
    public class AddOrderUseCase
    {
        private readonly IUnitOfWork _uow;

        private readonly Guid DefaultCustomerId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        
        public AddOrderUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Order> ExecuteAsync(List<OrderItemInputDto> items, Guid? customerId = null)
        {
            var idCustomer = customerId ?? DefaultCustomerId;

            var customer = await _uow.CustomerRepository.GetByIdAsync(idCustomer);
            
            if (customer == null)
            {
                throw new ArgumentException("Cliente não encontrado.", nameof(customerId));
            }

            var order = new Order(customer.Id);

            foreach (var itemDto in items)
            {
                var orderItem = new OrderItem(order.Id, itemDto.ProductId, itemDto.Quantity, itemDto.UnitPrice);
                order.AddItem(orderItem);
            }

            _uow.OrderRepository.Create(order);
            await _uow.CommitAsync();
            return order;
        }
    }
}
