using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
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
        private readonly ITokenHelper _jwt;

        public CartManager(ICartDal cartDal, ICartItemDal cartItemDal, IProductService productService, IUserService userService, ITokenHelper jwt)
        {
            _cartDal = cartDal;
            _itemDal = cartItemDal;
            _productService = productService;
            _userService = userService;
            _jwt = jwt;
        }

        public Users? AuthorizeId()
        {
            string? email = _jwt.GetUserEmailFromToken();
            if (email == null || email == "") return null;
            return _userService.GetByMail(email);
        }

        public IResult AddToCart(List<AddToCartDetail> addToCartDetail)
        {
            var user = AuthorizeId();
            if (user == null) return new ErrorResult(Messages.UserNotFound);

            var cart = GetCartByUserId(user.Id);
            if (cart == null)
            {
                CreateCart(user.Id);
                cart = GetCartByUserId(user.Id);
            }

            foreach (var item in addToCartDetail)
            {
                var result = _productService.GetProductStockDetails(item.ProductStockId);
                if (!result.Data.Any()) return new ErrorResult(Messages.ProductNotFound);

                var productStocks = result.Data[0];
                if (productStocks.UnitsInStock <= 0) return new ErrorResult(Messages.OutOfStock);

                List<CartItem> cartItems = _itemDal.GetAll().Where(ci => cart.CartItems.Contains(ci.CartItemId)).ToList();

                if (cartItems.Any(ci => ci.ProductStockId == item.ProductStockId))
                {
                    CartItem cartItem = cartItems.Where(c => c.ProductStockId == item.ProductStockId).First();
                    cartItem.Quantity += item.Quantity;
                    if (cartItem.Quantity > productStocks.UnitsInStock)
                    {
                        continue;
                    }
                    if (cartItem.Quantity <= 0)
                    {
                        cartItem.Quantity = 0;
                        _itemDal.Delete(cartItem);
                        continue;
                    }
                    cart.TotalPrice -= cartItem.UnitPrice;
                    cartItem.UnitPrice = _productService.GetById(productStocks.ProductId).Data.UnitPrice * cartItem.Quantity;
                    cart.TotalPrice += cartItem.UnitPrice;
                    _itemDal.Update(cartItem);
                }
                else
                {
                    var result1 = CreateCartItem(item, productStocks);
                    if (!result1.Success) continue;
                    cart.CartItems.Add(result1.Data.CartItemId);
                    cart.TotalPrice += result1.Data.UnitPrice;
                }


                cart.UpdateDate = DateTime.Now;
                _cartDal.Update(cart);
            }
            return new SuccessResult("Ürün eklendi.");
        }

        public void CreateCart(int userId)
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
        }

        public IDataResult<CartItem> CreateCartItem(AddToCartDetail detail, ProductDetailDto productStocks)
        {
            CartItem cartItem = new CartItem();
            cartItem.ProductStockId = detail.ProductStockId;
            cartItem.Status = true;
            cartItem.Quantity = detail.Quantity;
            cartItem.UnitPrice = _productService.GetById(productStocks.ProductId).Data.UnitPrice * cartItem.Quantity;
            if (cartItem.Quantity > productStocks.UnitsInStock)
            {
                return new ErrorDataResult<CartItem>("Stok sınırına ulaşıldı.");
            }
            _itemDal.Add(cartItem);
            return new SuccessDataResult<CartItem>(cartItem);
        }


        public IResult ClearCart()
        {
            var user = AuthorizeId();
            if (user == null) return new ErrorResult(Messages.UserNotFound);

            var cart = GetCartByUserId(user.Id);
            if (cart != null) _cartDal.Delete(cart);
            CreateCart(user.Id);
            return new SuccessResult("Sepet boşaldı.");
        }

        public Cart GetCartByUserId(int userId)
        {
            return _cartDal.Get(c => c.UserId == userId);
        }

        public IDataResult<CartDto> GetCart()
        {
            var user = AuthorizeId();
            if (user == null) return new ErrorDataResult<CartDto>(Messages.UserNotFound);

            var result = _cartDal.GetCartsByUserId(user.Id);
            if (result == null) return new ErrorDataResult<CartDto>(Messages.EmptyCart);
            return new SuccessDataResult<CartDto>(result);
        }

        public IResult DeleteProduct(List<AddToCartDetail> itemDelete)
        {
            var user = AuthorizeId();
            if (user == null) return new ErrorResult(Messages.UserNotFound);

            foreach (var item in itemDelete)
            {
                var cart = _cartDal.Get(x => x.UserId == user.Id);
                if (cart == null || !cart.CartItems.Contains(item.ProductStockId)) return new ErrorResult();

                var result = _itemDal.Get(x => x.CartItemId == item.ProductStockId);
                if (result == null) return new ErrorResult("Sıkıntılı silme işlemi");

                _itemDal.Delete(result);
            }
            return new SuccessResult("Sepetten kaldırıldı.");
        }

        public IResult RemoveItem(int cartItemId)
        {
            var user = AuthorizeId();
            if (user == null) return new ErrorResult(Messages.UserNotFound);

            CartItem? cartItem = _itemDal.Get(ci => ci.CartItemId == cartItemId);
            _itemDal.Delete(cartItem);

            Cart cart = _cartDal.Get(z => z.UserId == user.Id);
            cart.CartItems.Remove(cartItemId);
            _cartDal.Update(cart);

            return new SuccessResult("Sepetten Kaldırıldı");
        }
    }
}