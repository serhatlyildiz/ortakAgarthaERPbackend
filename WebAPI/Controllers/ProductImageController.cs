using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        IProductImagesService _productImagesService;

        public ProductImageController(IProductImagesService productImagesService)
        {
            _productImagesService = productImagesService;
        }

        [HttpPost("add")]
        public IActionResult Add(ProductImages productImages)
        {
            var result = _productImagesService.Add(productImages);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(ProductImages productImages)
        {
            var result = _productImagesService.Delete(productImages);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(ProductImages productImages)
        {
            var result = _productImagesService.Update(productImages);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyproductid")]
        public IActionResult Get(int Productid)
        {
            var result = _productImagesService.GetByProductId(Productid);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
