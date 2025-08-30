using Microsoft.AspNetCore.Mvc;
using SystoQ.Application.UseCases.Orders;
using SystoQ.Application.UseCases.Orders.DTOs;

namespace SystoQ.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AddOrderUseCase _addOrderUseCase;

        public OrdersController(AddOrderUseCase addOrderUseCase)
        {
            _addOrderUseCase = addOrderUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderInputDto orderDto)
        {
            if (orderDto == null || orderDto.Items == null || !orderDto.Items.Any())
            {
                return BadRequest("Dados da venda são obrigatórios.");
            }

            try
            {
                var createOrder = await _addOrderUseCase.ExecuteAsync(orderDto.Items, orderDto.CustomerId);

                return CreatedAtAction(nameof(CreateOrder), new { id = createOrder.Id }, createOrder);
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
