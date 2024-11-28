using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;

namespace DataAccess.Abstract
{
    public interface ICartDal : IEntityRepository<Cart>
    {
        public List<CartDto> GetCarts();
        public CartDto GetCartByUserId(int userId);
    }
}
