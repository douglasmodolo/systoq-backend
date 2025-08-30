using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SystoQ.Api.DTOs.Customers;
using SystoQ.Api.DTOs.Products;
using SystoQ.Application.UseCases.Customers;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Customers;
using X.PagedList;

namespace SystoQ.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AddCustomerUseCase _addCustomerUseCase;
        private readonly GetAllCustomersUseCase _getAllCustomersUseCase;
        private readonly GetCustomerByIdUseCase _getCustomerByIdUseCase;
        private readonly UpdateCustomerUseCase _updateCustomerUseCase;
        private readonly PatchCustomerUseCase _patchCustomerUseCase;
        private readonly DeleteCustomerUseCase _deleteCustomerUseCase;
        private readonly IMapper _mapper;

        public CustomersController(AddCustomerUseCase addCustomerUseCase, 
            GetAllCustomersUseCase getAllCustomersUseCase, 
            GetCustomerByIdUseCase getCustomerByIdUseCase, 
            UpdateCustomerUseCase updateCustomerUseCase, 
            PatchCustomerUseCase patchCustomerUseCase, 
            DeleteCustomerUseCase deleteCustomerUseCase,
            IMapper mapper)
        {
            _addCustomerUseCase = addCustomerUseCase;
            _getAllCustomersUseCase = getAllCustomersUseCase;
            _getCustomerByIdUseCase = getCustomerByIdUseCase;
            _updateCustomerUseCase = updateCustomerUseCase;
            _patchCustomerUseCase = patchCustomerUseCase;
            _deleteCustomerUseCase = deleteCustomerUseCase;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers([FromQuery] CustomersParameters customersParameters)
        {
            var customers = await _getAllCustomersUseCase.ExecuteAsync(customersParameters);

            if (customers == null || !customers.Any())
            { 
                return NotFound("Nenhum cliente encontrado.");
            }

            return BuildPaginatedProductsResponse(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("ID do cliente é obrigatório.");
            }

            var customer = await _getCustomerByIdUseCase.ExecuteAsync(id);
            
            if (customer == null)
            {
                return NotFound("Cliente não encontrado.");
            }
            
            var customerDto = _mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
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
                var created = await _addCustomerUseCase.ExecuteAsync(request.Name, request.Email, request.PhoneNumber);

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

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomer(Guid id, CustomerDto customerDto)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("ID do cliente é obrigatório.");
            }

            if (customerDto == null || id != customerDto.Id)
            {
                return BadRequest("Dados do cliente são obrigatórios.");
            }

            var customer = _mapper.Map<Customer>(customerDto);

            var updatedCustomer = await _updateCustomerUseCase.ExecuteAsync(id, customer);

            var updatedCustomerDto = _mapper.Map<CustomerDto>(updatedCustomer);

            return Ok(updatedCustomerDto);
        }

        [HttpPatch("api/customers/{id}/partial")]
        public async Task<ActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<UpdateCustomerRequestDto> patchCustomerDoc)
        {
            if (Guid.Empty == id || patchCustomerDoc == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var existingCustomer = await _getCustomerByIdUseCase.ExecuteAsync(id);

            if (existingCustomer == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            var customerToPatch = _mapper.Map<UpdateCustomerRequestDto>(existingCustomer);
            patchCustomerDoc.ApplyTo(customerToPatch, ModelState);

            if (!TryValidateModel(customerToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(customerToPatch, existingCustomer);

            var updatedCustomer = await _patchCustomerUseCase.ExecuteAsync(
                id, 
                customerToPatch.Name, 
                customerToPatch.Email, 
                customerToPatch.PhoneNumber
            );

            if (updatedCustomer == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            var updatedCustomerDto = _mapper.Map<CustomerDto>(updatedCustomer);
            return Ok(updatedCustomerDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(Guid id)
        {
            if (Guid.Empty == id)
            {
                return BadRequest("ID do cliente é obrigatório.");
            }

            var deleted = await _deleteCustomerUseCase.ExecuteAsync(id);

            if (!deleted)
            {
                return NotFound("Cliente não encontrado.");
            }

            return NoContent();
        }

        #region privateMethods
        private ActionResult<IEnumerable<CustomerDto>> BuildPaginatedProductsResponse(IPagedList<Customer> customers)
        {
            var metadata = new
            {
                customers.TotalItemCount,
                customers.PageSize,
                customers.PageNumber,
                customers.PageCount,
                customers.HasNextPage,
                customers.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var customersDto = _mapper.Map<IEnumerable<CustomerDto>>(customers);

            return Ok(customersDto);
        }
        #endregion
    }
}
