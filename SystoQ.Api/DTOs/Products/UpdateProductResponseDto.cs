namespace SystoQ.Api.DTOs.Products
{
    public class UpdateProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public float Stock { get; set; }
    }
}
