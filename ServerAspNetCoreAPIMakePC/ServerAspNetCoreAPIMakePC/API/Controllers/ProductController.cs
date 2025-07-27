using ServerAspNetCoreAPIMakePC.Application.Interfaces;

namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService  _productService;
        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }


    }
}
