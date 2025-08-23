namespace SystoQ.Application.UseCases.Sales.DTOs
{
    public class SaleInputDto
    {
        public Guid? CustomerId { get; set; }

        public List<SaleItemInputDto> Items { get; set; } = new();
    }
}
