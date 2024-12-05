using Core.Entities;

namespace Entities.DTOs
{
    public class AddToCartForUsersDto : IDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public short Quantity { get; set; }
    }
}
