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

        [HttpGet("delete")]
        public IActionResult Delete(int productStockId)
        {
            var result = _productStocksService.Delete(productStockId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
