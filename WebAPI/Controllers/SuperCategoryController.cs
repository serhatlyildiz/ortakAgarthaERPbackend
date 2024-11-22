using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperCategoryController : ControllerBase
    {
        private ISuperCategoryService _superCategoryService;

        public SuperCategoryController(ISuperCategoryService superCategoryService)
        {
            _superCategoryService = superCategoryService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Swagger
            //Dependency chain --

            Thread.Sleep(1000);

            var result = _superCategoryService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(SuperCategory superCategory)
        {
            var result = _superCategoryService.Add(superCategory);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(int superCategoryID)
        {
            var result = _superCategoryService.Delete(superCategoryID);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(SuperCategory superCategory)
        {
            var result = _superCategoryService.Update(superCategory);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult Get(int superCategoryID)
        {
            var result = _superCategoryService.GetById(superCategoryID);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}