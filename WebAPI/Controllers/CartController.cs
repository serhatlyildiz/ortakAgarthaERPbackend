using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        [HttpPost("add-to-cart")]
        public ActionResult Add(AddToCartForUsersDto addToCartForUsersDto)
        {
            var result = _cartService.AddToCart(addToCartForUsersDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("clear-cart")]
        public IActionResult Clear(int userId)
        {
            var result = _cartService.ClearCart(userId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("your-cart")]
        public IActionResult YourCart(int userId)
        {
            var result = _cartService.GetCart(userId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}