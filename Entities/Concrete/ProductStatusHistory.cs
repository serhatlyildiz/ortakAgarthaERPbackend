using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class ProductStatusHistory : IEntity
    {
        [Key]
        public int HistoryId { get; set; }
        public int ProductId { get; set; }
        public bool IsActive { get; set; }
        public int ChangedBy { get; set; }
        public DateTime ChangeDate { get; set; }
        public string? Remarks { get; set; }
    }
}
