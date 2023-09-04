using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartApi.Domain.Entities
{
    public class Cart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(ApiUser.Id))]
        public long UserId { get; set; }

        public ICollection<Item> CartItems { get; set; } = new List<Item>();
    }
}
