namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class CookieController : ControllerBase
    {
        [HttpGet("set")]
        public IActionResult SetCookie()
        {
            var cookieOptions = new CookieOptions   
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("MyCookie", "CookieValue", cookieOptions);
            return Ok("Cookie has been set.");
        }
        [HttpGet("get")]

        public IActionResult GetCookie()
        {
            var value = Request.Cookies["MyCookie"];
            if (value != null)
            {
                return Ok($"Cookie value: {value}");
            }
            return NotFound("Cookie not found.");
        }
        [HttpGet("delete")]
        public IActionResult DeleteCookie()
        {
            Response.Cookies.Delete("MyCookie");
            return Ok("Cookie has been deleted.");
        }
    }
}
