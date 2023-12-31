﻿using CartApi.Dtos.Write;

namespace CartApi.Utilities
{
    public class CartItemRequestBody
    {
        public ItemWriteDto Item { get; set; }

        public ItemActionType ActionType { get; set; }
    }
}
