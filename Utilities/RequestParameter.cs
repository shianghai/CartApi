using System;

namespace CartApi.Utilities
{
    public class RequestParameter
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? Quantity { get; set; }

        public string ItemName { get; set; }

        public string PhoneNumber { get; set; }
    }
}


//TODO: Get Clarification on the the [item] filter parameter for getting all cart items;
