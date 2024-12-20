using Core.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        private SalahTime _salahTime;

        public HelperController(SalahTime salahTime)
        {
            _salahTime = salahTime;
        }

        [HttpGet("get-salah-time")]
        public IActionResult GetSalahTime()
        {
            var result = _salahTime.GetSalahTimes();

            if (result == null) return BadRequest();
            return Ok(result);
        }
    }
}
