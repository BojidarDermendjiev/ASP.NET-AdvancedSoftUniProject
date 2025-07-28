namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Domain.Entities;
    using Application.Interfaces;
    using Application.DTOs.Product;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService  _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }

        /// <summary>
        /// Gets all products.
        /// GET /api/product
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        /// <summary>
        /// Gets a product by its unique identifier.
        /// GET /api/product/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        /// <summary>
        /// Creates a new product.
        /// POST /api/product
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] CreateProductDto dto)
        {
            try
            {
                var product = await this._productService.CreateProductAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return Conflict(new { error = ex.Message });
            }
          
        }
        /// <summary>
        /// Updates an existing product.
        /// PUT /api/product/{id}
        /// </summary>
        public async Task<ActionResult<Product>> Update(Guid id, [FromBody] UpdateProductDto dto)
        {
            try
            {
                var product = await this._productService.UpdateProductAsync(id, dto);
                if (product is null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Deletes a product by its ID.
        /// DELETE /api/product/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        //GET /api/product/search?q=laptop
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> Search([FromQuery] string q)
        {
            var results = await _productService.SearchProductsAsync(q ?? string.Empty);
            return Ok(results);
        }

        [HttpGet("paged")]
        public async Task<ActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (page < 1 || size < 1) return BadRequest("Page and size must be greater than 0.");
            var (products, totalCount) = await _productService.GetProductsPagedAsync(page, size);

            var response = new
            {
                Page = page,
                PageSize = size,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)size),
                Items = products
            };

            return Ok(response);
        }
    }
}
