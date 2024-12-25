using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Entities.Concrete;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStocksController : ControllerBase
    {
        private readonly IProductStocksService _productStocksService;

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

        [HttpGet("getlast")]
        public ActionResult<int> GetLastProductStockId()
        {
            var lastProductStockId = _productStocksService.GetLastProductStockId();
            if (lastProductStockId != -1)
            {
                return Ok(lastProductStockId);
            }
            return NotFound("Son kayıt bulunamadı.");
        }

        [HttpPost("add")]
        public IActionResult Add(ProductStocks productStocks)
        {
            var result = _productStocksService.Add(productStocks);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _productStocksService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productStocksService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getallbyproductdetailsidandcolor")]
        public IActionResult GetAllByProductDetailsIdAndColor(int productDetailsId, int productColorId)
        {
            var result = _productStocksService.GetAllByProductDetailsIdAndColor(productDetailsId, productColorId);
            if (result != null && result.Count > 0)
            {
                return Ok(result);
            }
            return NotFound("Belirtilen kriterlere uygun kayıt bulunamadı.");
        }

        [HttpGet("getallbyproductdetailsid")]
        public IActionResult GetAllByProductDetailsId(int productDetailsId)
        {
            var result = _productStocksService.GetAllByProductDetailsId(productDetailsId);
            if (result != null && result.Count > 0)
            {
                return Ok(result);
            }
            return NotFound("Belirtilen kriterlere uygun kayıt bulunamadı.");
        }
    }
}
