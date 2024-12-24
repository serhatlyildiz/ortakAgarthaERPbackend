using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private IProductDetailsService _productDetailsService;

        public ProductDetailsController(IProductDetailsService productDetailsService)
        {
            _productDetailsService = productDetailsService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Swagger
            //Dependency chain --

            Thread.Sleep(1000);

            var result = _productDetailsService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(ProductDetails productDetails)
        {
            var result = _productDetailsService.Add(productDetails);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("delete")]
        public IActionResult Delete(int productDetailsId)
        {
            var result = _productDetailsService.Delete(productDetailsId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(ProductDetails productDetails)
        {
            var result = _productDetailsService.Update(productDetails);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getallbyproductid")]
        public IActionResult GetAllByProductId(int productId)
        {
            var result = _productDetailsService.GetAllByProductId(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getallbyproductidandsize")]
        public IActionResult GetAllByProductIdAndSize(int productId, string productSize)
        {
            var result = _productDetailsService.GetAllByProductIdAndSize(productId, productSize);
            if (result != null && result.Count > 0)
            {
                return Ok(new { Success = true, Data = result });
            }

            return BadRequest(new { Success = false, Message = "No records found for the given ProductId and ProductSize." });
        }

        [HttpGet("getbyproductid")]
        public IActionResult GetByProductId(int productId)
        {
            var result = _productDetailsService.GetByProductId(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("restore")]
        public IActionResult GetRestore(int productId)
        {
            var result = _productDetailsService.GetByProductId(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}