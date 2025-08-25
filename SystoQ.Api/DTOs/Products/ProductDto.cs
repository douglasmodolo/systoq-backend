using System.ComponentModel.DataAnnotations;

namespace SystoQ.Api.DTOs.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(80, ErrorMessage = "O nome deve ter entre 1 e 80 caracteres", MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(300, ErrorMessage = "A descrição deve ter entre 1 e 300 caracteres", MinimumLength = 1)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório")]
        public decimal Price { get; set; }
        
        public int Stock { get; set; }
    }
}
