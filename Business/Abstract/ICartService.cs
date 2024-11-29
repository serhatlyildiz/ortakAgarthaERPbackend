using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface ICartService
    {
            IDataResult<Cart> GetCartByUserId(int userId);
            IResult AddToCart(AddToCartForUsersDto addToCartForUsers);
            IResult RemoveFromCart(AddToCartForUsersDto addToCartForUsers);
            IResult ClearCart(int userId);
            IDataResult<decimal> GetTotalPrice(int userId);
    }
}