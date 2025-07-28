using ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart;

namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Application.Interfaces;


    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this._shoppingCartService = shoppingCartService;
        }

        /// <summary>
        /// Gets the shopping cart for a specified user.
        /// </summary>
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetCartByUserId(Guid userId)
        {
            var cart = await this._shoppingCartService.GetCartByUserIdAsync(userId);
            if (cart is null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        /// <summary>
        /// Adds an item to the user's shopping cart.
        /// </summary>
        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddBasketItemDto dto)
        {
            try
            {
                var cart = await this._shoppingCartService.AddItemAsync(dto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Removes an item from the user's shopping cart.
        /// </summary>
        [HttpDelete("items")]
        public async Task<IActionResult> RemoveItem([FromBody] RemoveBasketItemDto dto)
        {
            try
            {
                var cart = await this._shoppingCartService.RemoveItemAsync(dto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Clears the user's shopping cart.
        /// </summary>
        [HttpDelete("items")]
        public async Task<IActionResult> ClearCart(Guid userId)
        {
            try
            {
                await this._shoppingCartService.ClearCartAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
