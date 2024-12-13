using Core.Entities;

namespace Entities.DTOs
{
    public class ProductDto : IDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int SuperCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SuperCategoryName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Status { get; set; }
        public string ProductCode { get; set; }
    }
}
