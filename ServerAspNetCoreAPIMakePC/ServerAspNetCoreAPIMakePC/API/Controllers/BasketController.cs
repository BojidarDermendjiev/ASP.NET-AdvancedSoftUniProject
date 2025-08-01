namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Application.Interfaces;
    using Application.DTOs.Basket;
    using static Domain.ErrorMessages.ErrorMessages;

    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            this._basketService = basketService;
        }

        /// <summary>
        /// Retrieves all baskets.
        /// GET /api/basket
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var baskets = await this._basketService.GetAllAsync();
            return Ok(baskets);
        }

        /// <summary>
        /// Retrieves a specific basket by its ID.
        /// GET /api/basket/{id}
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var basket = await this._basketService.GetByIdAsync(id);
            if (basket == null)
            {
                return NotFound();
            }

            return Ok(basket);
        }

        /// <summary>
        /// Retrieves a basket by user ID.
        /// GET /api/basket/user/{userId}
        /// </summary>
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var basket = await this._basketService.GetByUserIdAsync(userId);
            if (basket == null)
            {
                return NotFound();
            }

            return Ok(basket);
        }

        /// <summary>
        /// Creates a new basket.
        /// POST /api/basket
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBasketDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await this._basketService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing basket.
        /// PUT /api/basket/{id}
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBasketDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(string.Format(BasketMismatch));
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = await this._basketService.UpdateAsync(dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a basket by its ID.
        /// DELETE /api/basket/{id}
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this._basketService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
