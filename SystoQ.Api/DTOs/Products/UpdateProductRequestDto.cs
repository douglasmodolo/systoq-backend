using System.ComponentModel.DataAnnotations;

namespace SystoQ.Api.DTOs.Products
{
    public class UpdateProductRequestDto : IValidatableObject
    {
        public string? Name { get; set; }

        [MaxLength(250, ErrorMessage = "A descrição não pode exceder 250 caracteres")]
        public string? Description { get; set; }
        
        [Range(0.01, 9999.99, ErrorMessage = "O preço deve estar entre 0.01 e 9999.99")]
        public decimal? Price { get; set; }
                
        public int? Stock { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Stock < 0)
            {
                yield return new ValidationResult("O estoque não pode ser negativo", new[] { nameof(Stock) });
            }
        }
    }
}
