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
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public CartManager(ICartDal cartDal, ICartItemDal cartItemDal, IProductService productService, IUserService userService)
        {
            _cartDal = cartDal;
            _itemDal = cartItemDal;
            _productService = productService;
            _userService = userService;
        }

        public IResult AddToCart(AddToCartForUsersDto addToCartForUsers)
        {
            var user = _userService.GetById(addToCartForUsers.UserId);
            if (user == null) return new ErrorResult(Messages.UserNotFound);

            var result = _productService.GetProductStockDetails(addToCartForUsers.ProductStockId);
            if (!result.Data.Any()) return new ErrorResult(Messages.ProductNotFound);

            var productStocks = result.Data[0];
            if (productStocks.UnitsInStock <= 0) return new ErrorResult(Messages.OutOfStock);


            var cart = GetCartByUserId(addToCartForUsers.UserId);

            if (cart == null)
            {
                CreateCart(addToCartForUsers.UserId);
                cart = GetCartByUserId(addToCartForUsers.UserId);
            }

            List<CartItem> cartItems = _itemDal.GetAll().Where(ci => cart.CartItems.Contains(ci.CartItemId)).ToList();

            if (cartItems.Any(ci => ci.ProductStockId == addToCartForUsers.ProductStockId))
            {
                CartItem cartItem = cartItems.Where(c => c.ProductStockId == addToCartForUsers.ProductStockId).First();
                cartItem.Quantity += addToCartForUsers.Quantity;
                if (cartItem.Quantity > productStocks.UnitsInStock)
                {
                    return new ErrorResult("Stok sınırına ulaşıldı.");
                }
                if (cartItem.Quantity <= 0)
                {
                    cartItem.Quantity = 0;
                    _itemDal.Delete(cartItem);
                    return new ErrorResult("Ürün sepetten kaldırıldı.");
                }
                cartItem.UnitPrice = _productService.GetById(productStocks.ProductId).Data.UnitPrice * cartItem.Quantity;
                cart.TotalPrice += cartItem.UnitPrice;
                _itemDal.Update(cartItem);
            }
            else
            {
                var result1 = CreateCartItem(addToCartForUsers, productStocks);
                if (!result1.Success) return new ErrorResult("Stok sınırı aşıldı.");
                cart.CartItems.Add(result1.Data.CartItemId);
                cart.TotalPrice += result1.Data.UnitPrice;
            }


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
            cart.TotalPrice = 0;
            _cartDal.Add(cart);
            return new SuccessResult("Sepet Oluşturuldu.");
        }

        public IDataResult<CartItem> CreateCartItem(AddToCartForUsersDto addToCartForUsers, ProductDetailDto productStocks)
        {
            CartItem cartItem = new CartItem();
            cartItem.ProductStockId = addToCartForUsers.ProductStockId;
            cartItem.Status = true;
            cartItem.Quantity = addToCartForUsers.Quantity;
            cartItem.UnitPrice = _productService.GetById(productStocks.ProductId).Data.UnitPrice * cartItem.Quantity;
            if (cartItem.Quantity > productStocks.UnitsInStock)
            {
                return new ErrorDataResult<CartItem>("Stok sınırına ulaşıldı.");
            }
            _itemDal.Add(cartItem);
            return new SuccessDataResult<CartItem>(cartItem);
        }


        public IResult ClearCart(int userId)
        {
            var cart = GetCartByUserId(userId);
            if (cart != null) _cartDal.Delete(cart);
            CreateCart(userId);
            return new SuccessResult("Sepet boşaldı.");
        }

        public Cart GetCartByUserId(int userId)
        {
            return _cartDal.Get(c => c.UserId == userId);
        }

        public IDataResult<CartDto> GetCart(int userId)
        {
            var result = _cartDal.GetCartsByUserId(userId);
            if (result == null) return new ErrorDataResult<CartDto>(result, Messages.EmptyCart);
            return new SuccessDataResult<CartDto>(result);
        }
    }
}