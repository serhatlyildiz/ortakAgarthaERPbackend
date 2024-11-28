using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class Cart : IEntity
    {
        [Key]
        public int CartId { get; set; }
        public List<int> CartItemId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Status { get; set; }
    }
}