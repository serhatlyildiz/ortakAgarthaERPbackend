using Core.Entities;

namespace Entities.DTOs
{
    public class ProductDto : IDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsActive { get; set; }
        public List<string>? Images { get; set; }
    }

}
