using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class Customer:IEntity
    {
        [Key]
        public string? CustomerId { get; set; }
        public string? ContactName { get; set; }
        public string? CompanyName { get; set; }
        public string? City { get; set; }
    }
}
