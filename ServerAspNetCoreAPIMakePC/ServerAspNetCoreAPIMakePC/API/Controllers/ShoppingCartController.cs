using Microsoft.AspNetCore.Authorization;

namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    using Application.DTOs.ShoppingCart;
    using Application.Interfaces;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            this._shoppingCartService = shoppingCartService;
        }

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

        [HttpDelete("items")]
        public async Task<IActionResult> RemoveItem([FromBody] RemoveBasketItemDto dto)
        {
            var cart = await this._shoppingCartService.RemoveItemAsync(dto);
            return Ok(cart);
        }

        [HttpDelete("items/all/{userId:guid}")]
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