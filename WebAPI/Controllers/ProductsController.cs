using Business.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //Loosely coupled
        //naming convention
        //IoC Container -- Inversion of Control
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            Thread.Sleep(1000);

            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getproductswithtotalstock")]
        public IActionResult GetProductTotalStock()
        {
            Thread.Sleep(1000);

            var result = _productService.GetProductsWithTotalStock();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult Get(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyproductcodeforproductdto")]
        public IActionResult GetByProductCodeForProductDto(string productCode)
        {
            var result = _productService.GetByProductCodeForProductDto(productCode);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getallbycategory")]
        public IActionResult GetAllByCategory(int categoryId)
        {
            var result = _productService.GetAllByCategoryId(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("getproductdetails")]
        public IActionResult GetProductDetails()
        {
            var result = _productService.GetProductDetails();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }       
        
        
        [HttpGet("getproductdetails2")]
        public IActionResult GetProductDetails2()
        {
            var result = _productService.GetProductDetails2();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyproductidforproductdetails2")]
        public IActionResult GetByProductIdForProductDetails2(int productId)
        {
            var result = _productService.GetByProductIdForProductDetails2(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbycategoryidproductdetails2")]
        public IActionResult GetByCategoryIdProductDetails2(int superCategoryId, int? categoryId)
        {
            var result = _productService.GetByCategoryIdProductDetails2(superCategoryId, categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getproductdto")]
        public IActionResult GetProductDto()
        {
            var result = _productService.GetProductDto();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("getproductstockdetails")]
        public IActionResult GetProductStockDetails(int productStockId)
        {
            var result = _productService.GetProductStockDetails(productStockId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("get-product-stock-details-by-product")]
        public IActionResult GetProductStockDetailsByProduct(int productId)
        {
            var result = _productService.GetProductStockDetailsByProduct(productId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(ProductUpdateDto productUpdateDto)
        {
            var result = _productService.Add(productUpdateDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("productadd")]
        public IActionResult ProductAdd(Product product)
        {
            var result = _productService.ProductAdd(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("productstockadd")]
        public IActionResult ProductStockAdd(ProductStockAddDto productStockAddDto)
        {
            var result = _productService.ProductStockAdd(productStockAddDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("delete")]
        public IActionResult Delete(int productID)
        {
            var result = _productService.Delete(productID);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody] ProductUpdateDto productUpdateDto)
        {
            var result = _productService.Update(productUpdateDto.Product, productUpdateDto.ProductDetails, productUpdateDto.ProductStocks);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }



        [HttpPost("filter")]
        public IActionResult Filter([FromBody] ProductFilterModel filter)
        {
            var result = _productService.GetProductDetailsWithFilters(filter);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("restore")]
        public IActionResult Restore(int productID)
        {
            var result = _productService.Restore(productID);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
    }
}