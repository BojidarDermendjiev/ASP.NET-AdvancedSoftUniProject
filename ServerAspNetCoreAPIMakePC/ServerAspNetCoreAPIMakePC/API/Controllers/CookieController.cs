namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class CookieController : ControllerBase
    {
        /// <summary>
        /// Sets a secure, HTTP-only cookie named "MyCookie" with a fixed value.
        /// </summary>
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

        /// <summary>
        /// Retrieves the value of the "MyCookie" cookie if it exists.
        /// </summary>
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

        /// <summary>
        /// Deletes the "MyCookie" cookie from the response.
        /// </summary>
        [HttpGet("delete")]
        public IActionResult DeleteCookie()
        {
            Response.Cookies.Delete("MyCookie");
            return Ok("Cookie has been deleted.");
        }
    }
}
