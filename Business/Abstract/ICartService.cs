using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface ICartService
    {
        Cart GetCartByUserId(int userId);
        IResult AddToCart(List<AddToCartDetail> addToCartDetail);
        IResult ClearCart();
        IResult DeleteProduct(int productStocksId);
        IDataResult<CartDto> GetCart();
        IResult RemoveItem(int cartItemId);
    }
}