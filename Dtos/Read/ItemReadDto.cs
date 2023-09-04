namespace CartApi.Dtos.Read
{
    public class ItemReadDto
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
