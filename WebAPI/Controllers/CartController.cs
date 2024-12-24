using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("remove-item-from-cart")]
        public IActionResult RemoveItem(int cartItemId)
        {
            var result = _cartService.RemoveItem(cartItemId);

            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("add-to-cart")]
        public ActionResult Add(List<AddToCartDetail> addToCartDetail)
        {
            var result = _cartService.AddToCart(addToCartDetail);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("clear-cart")]
        public IActionResult Clear()
        {
            var result = _cartService.ClearCart();
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("clear-cart-item")]
        public IActionResult ClearItem(int productStockId)
        {
            var result = _cartService.RemoveItem(productStockId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("your-cart")]
        public IActionResult YourCart()
        {
            var result = _cartService.GetCart();
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}