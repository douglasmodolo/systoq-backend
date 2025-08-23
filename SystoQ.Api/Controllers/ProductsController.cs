using Microsoft.AspNetCore.Mvc;
using SystoQ.Api.DTOs.Products;
using SystoQ.Application.UseCases.Products;

namespace SystoQ.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AddProductUseCase _createProductUseCase;

        public ProductsController(AddProductUseCase createProductUseCase)
        {
            _createProductUseCase = createProductUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (request == null)
            {
                return BadRequest("Dados do produto são obrigatórios.");
            }
            try
            {
                var created = await _createProductUseCase.ExecuteAsync(request.Name, request.Description, request.Price, request.Stock);
                
                var response = new ProductDto
                {
                    Id = created.Id,
                    Name = created.Name,
                    Description = created.Description,
                    Price = created.Price,
                    Stock = created.Stock
                };

                return CreatedAtAction(nameof(CreateProduct), new { id = created.Id }, response);
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
