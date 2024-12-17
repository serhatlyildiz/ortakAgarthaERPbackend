using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class ProductStockAddDto : IDto
    {
        public int ProductDetailsId { get; set; }
        public int ProductId { get; set; }
        public int ProductStocksId { get; set; }
        public int ProductColorId { get; set; }
        public string? ProductSize { get; set; }
        public bool Status { get; set; }
        public string? ProductCode { get; set; }
        public short UnitsInStock { get; set; }
        public List<string>? Images { get; set; }
    }
}
