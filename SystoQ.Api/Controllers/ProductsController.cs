using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SystoQ.Api.DTOs.Products;
using SystoQ.Application.UseCases.Products;
using SystoQ.Domain.Entities;
using SystoQ.Domain.Filters.Products;
using X.PagedList;

namespace SystoQ.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AddProductUseCase _addProductUseCase;
        private readonly GetAllProductsUseCase _getAllProductsUseCase;
        private readonly GetProductByIdUseCase _getProductByIdUseCase;
        private readonly GetProductsByPriceRangeUseCase _getProductsByPriceRangeUseCase;
        private readonly SearchProductsByNameUseCase _searchProductsByNameUseCase;
        private readonly UpdateProductUseCase _updateProductUseCase;
        private readonly PatchProductUseCase _patchProductUseCase;
        private readonly DeleteProductUseCase _deleteProductUseCase;
        private readonly IMapper _mapper;

        public ProductsController(AddProductUseCase createProductUseCase, 
            GetAllProductsUseCase getAllProductsUseCase, 
            GetProductByIdUseCase getProductByIdUseCase,
            GetProductsByPriceRangeUseCase getProductsByPriceRangeUseCase,
            SearchProductsByNameUseCase searchProductsByNameUseCase,
            UpdateProductUseCase updateProductUseCase,
            PatchProductUseCase patchProductUseCase,
            DeleteProductUseCase deleteProductUseCase,
            IMapper mapper)
        {
            _addProductUseCase = createProductUseCase;
            _getAllProductsUseCase = getAllProductsUseCase;
            _getProductByIdUseCase = getProductByIdUseCase;
            _getProductsByPriceRangeUseCase = getProductsByPriceRangeUseCase;
            _searchProductsByNameUseCase = searchProductsByNameUseCase;
            _updateProductUseCase = updateProductUseCase;
            _patchProductUseCase = patchProductUseCase;
            _deleteProductUseCase = deleteProductUseCase;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts([FromQuery] ProductsParameters productsParameters)
        {
            var products = await _getAllProductsUseCase.ExecuteAsync(productsParameters);

            if (products == null || !products.Any())
            { 
                return NotFound("Nenhum produto encontrado.");
            }

            return BuildPaginatedProductsResponse(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID do produto é obrigatório.");
            }

            var product = await _getProductByIdUseCase.ExecuteAsync(id);
            if (product == null)
            {
                return NotFound($"Produto não encontrado.");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [HttpGet("/api/products/filter/price")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByPriceRange([FromQuery] ProductsPriceFilter filter)
        {
            var products = await _getProductsByPriceRangeUseCase.ExecuteAsync(filter);

            if (products == null || !products.Any())
            {
                return NotFound("Nenhum produto encontrado para o filtro de preço fornecido.");
            }

            return BuildPaginatedProductsResponse(products);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Search([FromQuery] ProductSearchFilter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Name))
            {
                return BadRequest("O parâmetro 'name' é obrigatório para a pesquisa.");
            }

            var products = await _searchProductsByNameUseCase.ExecuteAsync(filter);


            if (products == null || !products.Any())
            {
                return NotFound("Nenhum produto encontrado para a consulta de pesquisa fornecida.");
            }
            
            return BuildPaginatedProductsResponse(products);
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
                var createdProduct = await _addProductUseCase.ExecuteAsync(request.Name, request.Description, request.Price, request.Stock);

                var createdProductDto = _mapper.Map<ProductDto>(createdProduct);

                return CreatedAtAction(nameof(GetProductById), new { id = createdProductDto.Id }, createdProductDto);
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
        public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, ProductDto productDto)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID do produto é obrigatório.");
            }

            if (productDto == null || id != productDto.Id)
            {
                return BadRequest("Produto inválido ou ID não corresponde.");
            }

            var product = _mapper.Map<Product>(productDto);

            var updatedProduct = await _updateProductUseCase.ExecuteAsync(id, product);

            var updatedProductDto = _mapper.Map<ProductDto>(updatedProduct);

            return Ok(updatedProductDto);
        }

        [HttpPatch("/api/products/{id}/partial")]
        public async Task<ActionResult> Patch(Guid id, JsonPatchDocument<UpdateProductRequestDto> patchProductDto)
        {
            if (patchProductDto == null || id == Guid.Empty)
                return BadRequest("Dados inválidos.");

            var existingProduct = await _getProductByIdUseCase.ExecuteAsync(id);

            if (existingProduct == null)
                return NotFound("Produto não encontrado.");

            var productToPatch = _mapper.Map<UpdateProductRequestDto>(existingProduct);//_mapper.Map<UpdateProductRequestDto>(product);
            patchProductDto.ApplyTo(productToPatch, ModelState);

            if (!TryValidateModel(productToPatch))
                return ValidationProblem(ModelState);

            _mapper.Map(productToPatch, existingProduct);

            var product = await _patchProductUseCase.ExecuteAsync(
                id,
                productToPatch.Name,
                productToPatch.Description,
                productToPatch.Price,
                productToPatch.Stock
            );

            if (product == null)
                return NotFound("Produto não encontrado após a atualização.");

            var updatedProductDto = _mapper.Map<UpdateProductResponseDto>(product);

            return Ok(updatedProductDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("ID do produto é obrigatório.");
            }
            
            var deleted = await _deleteProductUseCase.ExecuteAsync(id);

            if (!deleted)
                return NotFound("Produto não encontrado.");

            return NoContent();
        }

        #region privateMethods
        private ActionResult<IEnumerable<ProductDto>> BuildPaginatedProductsResponse(IPagedList<Product> products)
        {
            var metadata = new
            {
                products.TotalItemCount,
                products.PageSize,
                products.PageNumber,
                products.PageCount,
                products.HasNextPage,
                products.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(productsDto);
        }
        #endregion
    }
}
