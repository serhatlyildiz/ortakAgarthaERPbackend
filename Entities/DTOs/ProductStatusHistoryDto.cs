using Core.Entities;

namespace Entities.DTOs
{
    public class ProductStatusHistoryDto : IDto
    {
        public int HistoryId { get; set; }
        public int ProductStockId { get; set; }
        public int ProductId { get; set; }
        public int ProductDetailsId { get; set; }
        public bool Status { get; set; }
        public int ChangedBy { get; set; }
        public string ProductCode { get; set; }
        public string ChangedByFirstName { get; set; }
        public string ChangedByLastName { get; set; }
        public string Email { get; set; }
        public DateTime ChangeDate { get; set; }
        public string Operations { get; set; }
        public string? Remarks { get; set; }
    }
}
