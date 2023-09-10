using System;
using System.ComponentModel.DataAnnotations;

namespace CartApi.Dtos.Write
{
    public class ItemWriteDto
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        
    }
}
