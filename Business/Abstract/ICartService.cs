using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Abstract
{
    public interface ICartService
    {
            Cart GetCartByUserId(int userId);
            IResult AddToCart(AddToCartForUsersDto addToCartForUsers);
            IResult ClearCart(int userId);
    }
}