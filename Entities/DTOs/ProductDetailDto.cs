﻿using Core.Entities;

namespace Entities.DTOs
{
    public class ProductDetailDto : IDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string ColorName { get; set; }
        public short UnitInStock { get; set; }
        public bool IsActive { get; set; }
    }

}
