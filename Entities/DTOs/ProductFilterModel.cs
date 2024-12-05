using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class ProductFilterModel: IDto
    {
        public string? ProductName { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinStock { get; set; }
        public int? MaxStock { get; set; }
        public string? ColorName { get; set; }
        public string? ProductSize { get; set; }
        public string? SuperCategoryName { get; set; }
        public string? CategoryName { get; set; }
        public bool? Status { get; set; }
    }
}
