using AutoMapper;
using CartApi.Domain.Entities;
using CartApi.Dtos.Read;
using CartApi.Dtos.Write;
using CartApi.Interfaces;
using CartApi.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Controllers
{
    
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IGenericRepository<Item> _itemRepo;
        private readonly IGenericRepository<Domain.Entities.Cart> _cartRepo;
        private readonly IAuthManager _authManager;



        public CartController(IGenericRepository<Item> itemRepo, IGenericRepository<Domain.Entities.Cart> cartRepo, IAuthManager authManager)
        {
            _itemRepo = itemRepo;
            _cartRepo = cartRepo;
            _authManager = authManager;
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("item/add")]
        public async Task<IActionResult> AddItem([FromBody] CartItemRequestBody cartItemRequestBody)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            var itemWriteDto = cartItemRequestBody.Item;
            var itemQuantity = cartItemRequestBody.Quantity;

            ItemReadDto itemReadDto;


            var token = Token.GetTokenFromRequest(Request);
            
            var user = await _authManager.GetUserFromTokenAsync(token);
            if (user == null) return Unauthorized("Could not get credentials of signed in user");
            var cart = await _cartRepo.Get(c => c.UserId == user.Id, new List<string> { "CartItems" });
            

            if (cart == null)
            {
                cart = new Domain.Entities.Cart();
                itemWriteDto.Quantity = itemQuantity;
                var cartItem = _itemRepo.Mapper.Map<Item>(itemWriteDto);
                cartItem.AddedDate = DateTime.Now;
                var cartItems = _itemRepo.Mapper.Map<IEnumerable<Item>>(cartItem).ToList();
                cart.UserId = user.Id;
                cart.CartItems = cartItems;

                var insertedCart =  await _cartRepo.Insert(cart);

                itemReadDto = _itemRepo.Mapper.Map<ItemReadDto>(cartItem);
                
                var res = new Response<ItemReadDto>
                {
                    Status = true,
                    Message = "Item Added successfully",
                    Data = itemReadDto
                };

                return Created("GetItemById" , res);
            }
            else
            {
                CartApi.Domain.Entities.Cart updatedCart;
                var cartItem = _cartRepo.Mapper.Map<Item>(itemWriteDto);
                cartItem.AddedDate = DateTime.Now;
                var oldCartItem = cart.CartItems.FirstOrDefault(c => c.ItemId == itemWriteDto.ItemId);
                if(oldCartItem == null)
                {
                    cartItem.Quantity = itemQuantity;
                    cart.CartItems.Add(cartItem);
                }
                else
                {
                    oldCartItem.Quantity += itemQuantity;
                }

                updatedCart = await _cartRepo.Update(cart);

                itemReadDto = _itemRepo.Mapper.Map<ItemReadDto>(cartItem);

                var res = new Response<ItemReadDto>
                {
                    Status = true,
                    Message = "Item Added successfully",
                    Data = itemReadDto
                };

                return Created("GetItemById", res);
            }

            
           
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("item/remove/{itemId}")]
        public async Task<IActionResult> RemoveCartItemByIdAsync([FromRoute] int itemId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token = Token.GetTokenFromRequest(Request);
            
            var user = await _authManager.GetUserFromTokenAsync(token);
            if (user == null) return Unauthorized("Could not get credentials of signed in user");

            var cart = await _cartRepo.Get(c => c.UserId == user.Id, new List<string>{ "CartItems"});

            var cartItem = cart.CartItems.FirstOrDefault(x => x.ItemId == itemId);

            if(cartItem != null)
            {
                bool isRemoved = cart.CartItems.Remove(cartItem);
                if(isRemoved)
                {
                   var updatedCart =  await _cartRepo.Update(cart);

                    var cartReadDtos = _cartRepo.Mapper.Map<ItemReadDto>(cartItem);

                    var resultObject = new Response<ItemReadDto>
                    {
                        Status = true,
                        Message = "Item Removed successfully",
                        Data = cartReadDtos
                    };

                    return Ok(resultObject);
                }
                else
                {
                    throw new Exception($"Could Not Remove cart item for item with id {itemId}");
                }

            }
            var res = new Response<ItemWriteDto>
            {
                Status = false,
                Message = $"Item with id {itemId} does not exist in the cart of the signed in user",
                Data = null
            };

            return BadRequest(res);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("item/{id}", Name = "GetItemById")]
        public async Task<IActionResult> GetByItemId([FromRoute] int id)
        {
            var token = Token.GetTokenFromRequest(Request);
            
            var user = await _authManager.GetUserFromTokenAsync(token);

            if (user == null) return Unauthorized("Could not get credentials of signed in user");

            var cart = await _cartRepo.Get(c => c.UserId == user.Id, new List<string> { "CartItems" });

            var cartItem = cart?.CartItems?.FirstOrDefault(x => x.ItemId == id);
            if(cartItem != null)
            {
                var cartItems = _cartRepo.Mapper.Map<IEnumerable<Item>>(cartItem);

                var data = _cartRepo.Mapper.Map<ItemReadDto>(cartItem);
                var response = new Response<ItemReadDto>
                {
                    Status = true,
                    Message = "Item Retrieved Successfully",
                    Data = data
                };
                return Ok(response);
            }
            else
            {
                var res = new Response<ItemReadDto>
                {
                    Status = false,
                    Message = $"Cart Item with id {id} does not exist in cart of the signed in user",
                    Data = null
                };
                return NotFound(res);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("item/all")]
        public async Task<IActionResult> GetAllByFilters([FromQuery] RequestParameter parameters)
        {
            var token = Token.GetTokenFromRequest(Request);
            
            var user = await _authManager.GetUserFromTokenAsync(token);
            if (user == null) return Unauthorized("Could not get credentials of signed in user");

            var cart = await _cartRepo.Get(c => c.UserId == user.Id, new List<string> { "CartItems" });

            IEnumerable<Item> query = cart.CartItems;

            if(parameters.FromDate != null)
            {
                query = query.Where(i => i.AddedDate >= parameters.FromDate);
            }
            if(parameters.ToDate != null)
            {
                query = query.Where(i => i.AddedDate <= parameters.ToDate);
            }
            if(parameters.Quantity > 0)
            {
                query = query.Where(item => item.Quantity >= parameters.Quantity);
            }

            var cartItems = query.ToList();

            if(cartItems.Count > 0)
            {
                var mappedCartItems = _itemRepo.Mapper.Map<IEnumerable<ItemReadDto>>(cartItems);


                var response = new Response<IEnumerable<ItemReadDto>>
                {
                    Status = true,
                    Message = "Cart Items Retrieved Successfully",
                    Data = mappedCartItems
                };

                return Ok(response);
            }
            var res = new Response<ItemReadDto>()
            {
                Status = false,
                Message = "No Items match the search filters provided",
                Data = null
            };
            return NotFound(res);
            
        }
    }
}


