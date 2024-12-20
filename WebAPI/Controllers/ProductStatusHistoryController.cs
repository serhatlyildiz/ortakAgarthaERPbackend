using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStatusHistoryController : ControllerBase
    {
        IProductStatusHistoryService _productStatusHistoryService;
        public ProductStatusHistoryController(IProductStatusHistoryService productStatusHistoryService)
        {
            _productStatusHistoryService = productStatusHistoryService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            Thread.Sleep(1000);

            var result = _productStatusHistoryService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getproductstatushistorywithdetails")]
        public IActionResult GetProductStatusHistoryWithDetails(
    int? productStockId = null,
    DateTime? startDate = null,
    DateTime? endDate = null,
    string? productCode = null, string? operations = null)
        {
            var result = _productStatusHistoryService.GetProductStatusHistoryWithDetails(
                productStockId, startDate, endDate, productCode, operations);

            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("add")]
        public IActionResult Add(ProductStatusHistory productStatusHistory)
        {
            var result = _productStatusHistoryService.Add(productStatusHistory);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
