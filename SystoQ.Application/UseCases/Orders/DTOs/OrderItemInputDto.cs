namespace SystoQ.Application.UseCases.Orders.DTOs
{
    public class OrderItemInputDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
