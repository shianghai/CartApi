using CartApi.Domain.Entities;
using CartApi.Dtos.Read;
using CartApi.Interfaces;
using CartApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Services
{
    public class CartService : ICartService
    {
        private readonly IGenericRepository<Item> _itemRepo;
        private readonly IGenericRepository<Domain.Entities.Cart> _cartRepo;
        private readonly IAuthManager _authManager;
        public CartService(IGenericRepository<Item> itemRepo, IGenericRepository<Domain.Entities.Cart> cartRepo, IAuthManager authManager)
        {
            _itemRepo = itemRepo;
            _cartRepo = cartRepo;
            _authManager = authManager;
        }
        public async Task<ItemReadDto> AddCartItemAsync(CartItemRequestBody cartItemRequestBody, long userId)
        {
            
            var itemWriteDto = cartItemRequestBody.Item;
            var itemQuantity = cartItemRequestBody.Quantity;

            ItemReadDto itemReadDto = null;

            var cart = await _cartRepo.Get(c => c.UserId == userId, new List<string> { "CartItems" });


            if (cart == null)
            {
                cart = new Domain.Entities.Cart();
                itemWriteDto.Quantity = itemQuantity;
               
                
                cart.UserId = userId;
                var insertedCart = await _cartRepo.Insert(cart);

               


                if (insertedCart != null)
                {
                    var cartItem = _itemRepo.Mapper.Map<Item>(itemWriteDto);
                    cartItem.AddedDate = DateTime.Now;

                    var insertedCartItem = _itemRepo.Insert(cartItem);

                    if(insertedCartItem != null) 
                        itemReadDto = _itemRepo.Mapper.Map<ItemReadDto>(insertedCartItem);
                }


                return itemReadDto;
            }
            else
            {
                Item updatedItem;
                var cartItem = _cartRepo.Mapper.Map<Item>(itemWriteDto);
                cartItem.AddedDate = DateTime.Now;
                cartItem.CartId = cart.Id; 

                var oldCartItem = cart.CartItems.FirstOrDefault(c => c.ItemId == itemWriteDto.ItemId);
                if (oldCartItem == null)
                {
                    cartItem.Quantity = itemQuantity;
                    updatedItem = await _itemRepo.Insert(cartItem);
                }
                else
                {
                    oldCartItem.Quantity += itemQuantity;
                    updatedItem = await _itemRepo.Insert(oldCartItem);
                }
                
                if(updatedItem != null)
                {
                    itemReadDto = _itemRepo.Mapper.Map<ItemReadDto>(cartItem);
                }

                return itemReadDto;
            }
        }
        public async Task<Item> DeleteCartItemByIdAsync(int itemId, long userId)
        {
            var cart = await _cartRepo.Get(c => c.UserId == userId, new List<string> { "CartItems" });

            var cartItem = cart.CartItems.FirstOrDefault(x => x.ItemId == itemId);

            if(cartItem != null)
            {
                bool isDeleted = cart.CartItems.Remove(cartItem);
                if (isDeleted)
                {
                    var updatedCart = await _cartRepo.Update(cart);
                    return updatedCart != null ? cartItem : null;
                }
                return null;
                
            }
            return null;
        }

        public async  Task<ItemReadDto> GetCartItemByIdAsync(int id, long userId)
        {
            var cart = await _cartRepo.Get(c => c.UserId == userId, new List<string> { "CartItems" });

            var cartItem = cart?.CartItems?.FirstOrDefault(x => x.ItemId == id);
            var data = _cartRepo.Mapper.Map<ItemReadDto>(cartItem);
            return data;
        }

        public async Task<IEnumerable<ItemReadDto>> SearchCartItemsAsync(RequestParameter parameters, long userId)
        {
            var cart = await _cartRepo.Get(c => c.UserId == userId, new List<string> { "CartItems" });

            IEnumerable<Item> query = cart.CartItems;

            if (parameters.FromDate != null)
            {
                query = query.Where(i => i.AddedDate.Date >= parameters.FromDate.Value.Date);
            }
            if (parameters.ToDate != null)
            {
                query = query.Where(i => i.AddedDate.Date <= parameters.ToDate.Value.Date);
            }
            if (parameters.Quantity > 0)
            {
                query = query.Where(item => item.Quantity >= parameters.Quantity);
            }

            var cartItems = query.ToList();

            var mappedCartItems = _itemRepo.Mapper.Map<IEnumerable<ItemReadDto>>(cartItems);
            return mappedCartItems;

        }
    }

    public interface ICartService
    {
        Task<ItemReadDto> GetCartItemByIdAsync(int id, long userId);

        Task<ItemReadDto> AddCartItemAsync(CartItemRequestBody cartItemRequestBody, long id);

        Task<IEnumerable<ItemReadDto>> SearchCartItemsAsync(RequestParameter parameters, long userId);

        Task<Item> DeleteCartItemByIdAsync(int itemId, long userId);
    }
}
