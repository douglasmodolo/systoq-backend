using System.ComponentModel.DataAnnotations;

namespace SystoQ.Api.DTOs.Customers
{
    public class UpdateCustomerRequestDto : IValidatableObject
    {
        public string? Name { get; set; } = string.Empty;
        
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? PhoneNumber { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name) && Name.Length < 3)
            {
                yield return new ValidationResult(
                    "O nome deve ter ao menos 3 caracteres.", new[] { nameof(Name) }
                );
            }
        }
    }
}
