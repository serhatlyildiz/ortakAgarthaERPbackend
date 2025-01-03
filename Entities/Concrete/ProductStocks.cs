﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductStocks: IEntity
    {
        [Key]
        public int ProductStocksId { get; set; }
        public int ProductDetailsId { get; set; }
        public int ProductColorId { get; set; }
        public short UnitsInStock { get; set; }
        public List<string>? Images { get; set; }
        public bool Status { get; set; }
    }
}
