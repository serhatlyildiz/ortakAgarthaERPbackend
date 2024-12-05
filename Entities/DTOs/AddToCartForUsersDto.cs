using Core.Entities;

namespace Entities.DTOs
{
    public class AddToCartForUsersDto : IDto
    {
        public int UserId { get; set; }
        public int ProductStockId { get; set; }
        public short Quantity { get; set; }
    }
}
