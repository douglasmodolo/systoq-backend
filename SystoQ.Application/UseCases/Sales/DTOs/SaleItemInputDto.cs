namespace SystoQ.Application.UseCases.Sales.DTOs
{
    public class SaleItemInputDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
