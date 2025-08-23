using Microsoft.AspNetCore.Mvc;
using SystoQ.Application.UseCases.Sales;
using SystoQ.Application.UseCases.Sales.DTOs;

namespace SystoQ.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly CreateSaleUseCase _createSaleUseCase;

        public SalesController(CreateSaleUseCase createSaleUseCase)
        {
            _createSaleUseCase = createSaleUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] SaleInputDto saleDto)
        {
            if (saleDto == null || saleDto.Items == null || !saleDto.Items.Any())
            {
                return BadRequest("Dados da venda são obrigatórios.");
            }

            try
            {
                var createdSale = await _createSaleUseCase.ExecuteAsync(saleDto.Items, saleDto.CustomerId);

                return CreatedAtAction(nameof(CreateSale), new { id = createdSale.Id }, createdSale);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro ao processar a solicitação.");
            }
        }
    }
}
