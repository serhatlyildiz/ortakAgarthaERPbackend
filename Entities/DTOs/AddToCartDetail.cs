using Core.Entities;

namespace Entities.DTOs
{
    public class AddToCartDetail : IDto
    {
        public int ProductStockId { get; set; }
        public short Quantity { get; set; }
    }
}