namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    using Application.DTOs.Brand;
    using Application.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            this._brandService = brandService;
        }

        /// <summary>
        /// Retrieves all brands.
        /// GET /api/brand
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brands = await this._brandService.GetAllAsync();
            return Ok(brands);
        }


        /// <summary>
        /// Retrieves a specific brand by its ID.
        /// GET /api/brand/{id}
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await this._brandService.GetByIdAsync(id);
            if (brand is null)
            {
                return NotFound();
            }

            return Ok(brand);
        }

        /// <summary>
        /// Creates a new brand.
        /// POST /api/brand
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBrandDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await this._brandService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing brand.
        /// PUT /api/brand/{id}
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBrandDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(string.Format(IdMismatchUserFriendly));
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await this._brandService.UpdateAsync(dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a brand by its ID.
        /// DELETE /api/brand/{id}
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _brandService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
