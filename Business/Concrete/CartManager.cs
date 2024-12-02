using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartDal _cartDal;
        private ICartItemDal _itemDal;
        private readonly IProductDal _productDal;
        private readonly IProductService _productService;
        private readonly IUserDal _userDal;

        public CartManager(ICartDal cartDal, ICartItemDal cartItemDal, IProductDal productDal, IProductService productService, IUserDal userDal)
        {
            _cartDal = cartDal;
            _itemDal = cartItemDal;
            _productDal = productDal;
            _productService = productService;
            _userDal = userDal;
        }

        public IResult AddToCart(AddToCartForUsersDto addToCartForUsers)
        {
            var user = _userDal.Get(u => u.Id == addToCartForUsers.UserId);
            if (user == null) return new ErrorResult(Messages.UserNotFound);
            var product = _productDal.GetProductDetails().Where(p => p.ProductId == addToCartForUsers.ProductId || p.IsActive == true || p.UnitInStock >= 0);
            if (product == null) return new ErrorResult(Messages.ProductNotFound);


            Cart cart = GetCartByUserId(addToCartForUsers.UserId).Data;

            if (cart == null)
            {
                CreateCart(addToCartForUsers.UserId);
                cart = GetCartByUserId(addToCartForUsers.UserId).Data;
            }

            List<CartItem> cartItems = _itemDal.GetAll().Where(ci => cart.CartItems.Contains(ci.CartItemId)).ToList();

            if (cartItems.Any(ci => ci.ProductId == addToCartForUsers.ProductId))
            {
                CartItem cartItem = cartItems.Where(c => c.ProductId == addToCartForUsers.ProductId).First();
                cartItem.Quantity += 1;
                cartItem.UnitPrice = _productService.GetById(addToCartForUsers.ProductId).Data.UnitPrice * cartItem.Quantity;
                _itemDal.Update(cartItem);
            }
            else cart.CartItems.Add(CreateCartItem(addToCartForUsers.ProductId));

            cart.UpdateDate = DateTime.Now;
            _cartDal.Update(cart);
            return new SuccessResult("Ürün eklendi.");
        }

        public IResult CreateCart(int userId)
        {
            Cart cart = new Cart();
            cart.UserId = userId;
            cart.CartItems = [];
            cart.Status = true;
            cart.CreatedDate = DateTime.Now;
            cart.UpdateDate = DateTime.Now;
            cart.IsCompleted = false;
            _cartDal.Add(cart);
            return new SuccessResult("Sepet Oluşturuldu.");
        }
        public int CreateCartItem(int productId)
        {
            CartItem cartItem = new CartItem();
            cartItem.ProductId = productId;
            cartItem.Quantity = 1;
            cartItem.UnitPrice = _productService.GetById(productId).Data.UnitPrice;
            _itemDal.Add(cartItem);
            return cartItem.CartItemId;
        }

        public IResult ClearCart(int userId)
        {
            throw new NotImplementedException();
        }

        public IDataResult<Cart> GetCartByUserId(int userId)
        {
            return new SuccessDataResult<Cart>(_cartDal.Get(c => c.UserId == userId && c.Status == true));
        }

        public IDataResult<decimal> GetTotalPrice(int userId)
        {
            throw new NotImplementedException();
        }

        public IResult RemoveFromCart(AddToCartForUsersDto addToCartForUsers)
        {
            throw new NotImplementedException();
        }
    }

}