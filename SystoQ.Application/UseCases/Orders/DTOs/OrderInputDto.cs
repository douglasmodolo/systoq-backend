namespace SystoQ.Application.UseCases.Orders.DTOs
{
    public class OrderInputDto
    {
        public Guid? CustomerId { get; set; }

        public List<OrderItemInputDto> Items { get; set; } = new();
    }
}
