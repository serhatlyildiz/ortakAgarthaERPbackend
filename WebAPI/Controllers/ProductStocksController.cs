using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStocksController : ControllerBase
    {
        IProductStocksService _productStocksService;
        public ProductStocksController(IProductStocksService productStocksService)
        {
            _productStocksService = productStocksService;
        }
    }
}
