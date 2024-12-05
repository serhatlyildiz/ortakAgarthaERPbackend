using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCartDal : EfEntityRepositoryBase<Cart, NorthwindContext>, ICartDal
    {
        public List<CartDto> GetCarts()
        {
            using (var context = new NorthwindContext())
            {
                var result = context.Carts
                    .Select(c => new CartDto
                    {
                        CartId = c.CartId,
                        UserId = c.UserId,
                        CreatedDate = c.CreatedDate,
                        UpdateDate = c.UpdateDate,
                        TotalPrice = c.TotalPrice,
                        Status = c.Status,
                        CartItems = context.CartItems
                            .Where(ci => c.CartItems.Contains(ci.CartItemId)) // Doğru kontrol
                            .Select(ci => new CartItemDto
                            {
                                CartItemId = ci.CartItemId,
                                ProductId = ci.ProductStockId,
                                Quantity = ci.Quantity,
                                UnitPrice = ci.UnitPrice
                            }).ToList()
                    }).ToList();

                return result;
            }
        }



        public CartDto GetCartByUserId(int userId)
        {
            using (var context = new NorthwindContext())
            {
                var result = context.Carts
                    .Where(c => c.UserId == userId)
                    .Select(c => new CartDto
                    {
                        CartId = c.CartId,
                        UserId = c.UserId,
                        CreatedDate = c.CreatedDate,
                        UpdateDate = c.UpdateDate,
                        TotalPrice = c.TotalPrice,
                        Status = c.Status,
                        CartItems = context.CartItems.Where(ci => c.CartItems.Contains(ci.CartItemId))
                            .Select(ci => new CartItemDto
                            {
                                CartItemId = ci.CartItemId,
                                ProductId = ci.ProductStockId,
                                Quantity = ci.Quantity,
                                UnitPrice = ci.UnitPrice
                            }).ToList()
                    }).FirstOrDefault();

                return result;
            }
        }
    }

}
