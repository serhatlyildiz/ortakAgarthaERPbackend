using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private IColorsService _colorsService;

        public ColorsController(IColorsService colorsService)
        {
            _colorsService = colorsService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Swagger
            //Dependency chain --

            Thread.Sleep(1000);

            var result = _colorsService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Colors colors)
        {
            var result = _colorsService.Add(colors);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("delete")]
        public IActionResult Delete(int colorId)
        {
            var result = _colorsService.Delete(colorId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(Colors colors)
        {
            var result = _colorsService.Update(colors);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult Get(int colorsId)
        {
            var result = _colorsService.GetById(colorsId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}