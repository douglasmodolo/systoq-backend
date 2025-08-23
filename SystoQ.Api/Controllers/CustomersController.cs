using Microsoft.AspNetCore.Mvc;
using SystoQ.Api.DTOs.Customers;
using SystoQ.Application.UseCases.Customers;

namespace SystoQ.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CreateCustomerUseCase _createCustomerUseCase;

        public CustomersController(CreateCustomerUseCase createCustomerUseCase)
        {
            _createCustomerUseCase = createCustomerUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            if (request == null)
            {
                return BadRequest("Dados do cliente são obrigatórios.");
            }
            try
            {
                var created = await _createCustomerUseCase.ExecuteAsync(request.Name, request.Email, request.PhoneNumber);
                
                var response = new CustomerDto
                {
                    Id = created.Id,
                    Name = created.Name,
                    Email = created.Email,
                    PhoneNumber = created.PhoneNumber
                };
                
                return CreatedAtAction(nameof(CreateCustomer), new { id = created.Id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno do servidor: " + ex.Message);
            }
        }
    }
}
