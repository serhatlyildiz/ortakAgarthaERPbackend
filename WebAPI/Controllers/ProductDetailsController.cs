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
        public IActionResult getRestore(int productId)
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