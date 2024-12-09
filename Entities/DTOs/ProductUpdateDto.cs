using Core.Entities;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class ProductUpdateDto : IDto
    {
        public Product Product { get; set; }
        public ProductDetails ProductDetails { get; set; }
        public ProductStocks ProductStocks { get; set; }
    }
}
