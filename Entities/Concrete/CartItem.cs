using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class CartItem : IEntity
    {
        [Key]
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public short Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Status { get; set; }
    }
}