﻿using System;

namespace CartApi.Dtos.Write
{
    public class ItemWriteDto
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        
    }
}
