using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartApi.Domain.Entities
{
    public class Item
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }    

        public int Quantity { get; set; }

        public DateTime AddedDate { get; set; }

        public decimal UnitPrice { get; set; }

        [ForeignKey(nameof(CartApi.Domain.Entities.Cart))]
        public int CartId { get; set; }
        

    }
}
