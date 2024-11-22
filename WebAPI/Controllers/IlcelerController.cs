using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IlcelerController : ControllerBase
    {
        IilcelerService _ilcelerService;
        public IlcelerController(IilcelerService ilcelerService)
        {
            _ilcelerService = ilcelerService;
        }

        [HttpGet("getallbyillerid")]
        public IActionResult GetAllByIllerId(int ilId)
        {
            var result = _ilcelerService.GetAllByIllerId(ilId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult Get(int id)
        {
            var result = _ilcelerService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}