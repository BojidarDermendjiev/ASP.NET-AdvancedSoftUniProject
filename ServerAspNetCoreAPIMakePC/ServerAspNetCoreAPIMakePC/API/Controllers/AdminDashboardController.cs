namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    
    using Application.DTOs.Admin;

    [Area("Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        [HttpGet]
        public ActionResult<AdminDashboardDto> GetDashboardStats()
        {
            var model = new AdminDashboardDto
            {
                TotalUsers = 123,
                TotalOrders = 456,
                TotalProducts = 789
            };

            return Ok(model);
        }
    }
}
