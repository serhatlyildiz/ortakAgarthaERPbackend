﻿using Core.Entities;

namespace Entities.DTOs
{
    public class CartItemDto : IDto
    {
        public int CartItemId { get; set; }
        public int ProductStockId { get; set; }
        public short Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
