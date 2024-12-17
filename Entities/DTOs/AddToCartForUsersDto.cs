using Core.Entities;

namespace Entities.DTOs
{
    public class AddToCartForUsersDto : IDto
    {
        public int UserId { get; set; }
        public List<AddToCartDetail> Details { get; set; }
    }
}
