using Core.Entities;

namespace Entities.DTOs
{
    public class CartDto : IDto
    {
        public int CartId { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Status { get; set; }
    }
}
