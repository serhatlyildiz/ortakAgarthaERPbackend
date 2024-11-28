using Business.Abstract;
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

        public CartManager(ICartDal cartDal, ICartItemDal cartItemDal)
        {
            _cartDal = cartDal;
            _itemDal = cartItemDal;
        }

        public IResult AddToCart(AddToCartForUsersDto addToCartForUsers)
        {
            //// Kullanıcıya ait aktif sepeti al
            //var result = _cartDal.Get(c => c.UserId == addToCartForUsers.UserId && c.Status == false);

            //// Eğer sepet bulunursa
            //if (result != null)
            //{
            //    // Mevcut sepetteki ürünleri kontrol et
            //    var cartItems = result.CartItems;

            //    _itemDal.GetAll().FindAll(ci => ci);
            //    // Sepetteki ürünlerden gelen productId'yi arayın
            //    var existingItem = cartItems.FirstOrDefault(ci => ci.ProductId == addToCartForUsers.ProductId);

            //    // Eğer ürün zaten sepette varsa, sayısını artır
            //    if (existingItem != null)
            //    {
            //        existingItem.Quantity += 1; // Sayıyı 1 artırıyoruz
            //        _itemDal.Update(existingItem); // Güncellenmiş ürünü veritabanına kaydediyoruz
            //        return new SuccessResult("Ürün sepete eklendi ve sayısı artırıldı.");
            //    }
            //    else
            //    {
            //        // Eğer ürün sepette yoksa yeni bir CartItem oluşturun
            //        var newItem = new CartItem
            //        {
            //            ProductId = addToCartForUsers.ProductId,
            //            Quantity = 1,
            //            Price = addToCartForUsers.Price // Fiyat bilgisi de verilebilir
            //        };

            //        _itemDal.Add(newItem); // Yeni ürünü ekliyoruz
            //        result.ListCartItems.Add(newItem); // Sepet listesini güncelliyoruz
            //        _cartDal.Update(result); // Sepeti güncelliyoruz

            //        return new SuccessResult("Yeni ürün sepete eklendi.");
            //    }
            //}
            //else
            //{
            //    // Eğer sepet yoksa, yeni bir sepet oluştur
            //    var newCart = new Cart
            //    {
            //        UserId = addToCartForUsers.UserId,
            //        Status = false, // Durum aktif (0)
            //        CreatedDate = DateTime.Now,
            //        UpdatedDate = DateTime.Now,
            //        TotalPrice = 0, // Başlangıçta fiyatı sıfır
            //        ListCartItems = new List<CartItem>()
            //    };

            //    // Yeni CartItem ekle
            //    var newItem = new CartItem
            //    {
            //        ProductId = addToCartForUsers.ProductId,
            //        Quantity = 1,
            //        Price = addToCartForUsers.Price // Fiyat bilgisi
            //    };

            //    _itemDal.Add(newItem); // Yeni ürünü ekliyoruz
            //    newCart.ListCartItems.Add(newItem); // Sepet listesine ekliyoruz

            //    _cartDal.Add(newCart); // Yeni sepeti veritabanına ekliyoruz

            //    return new SuccessResult("Yeni sepet oluşturuldu ve ürün eklendi.");
            //}
            return null;
        }


        public IResult ClearCart(int userId)
        {
            throw new NotImplementedException();
        }

        public IDataResult<CartDto> GetCartByUserId(int userId)
        {
            throw new NotImplementedException();
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