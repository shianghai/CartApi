using AutoMapper;
using CartApi.Domain.Entities;
using CartApi.Dtos.Read;
using CartApi.Dtos.Write;
using CartApi.Interfaces;
using CartApi.Services;
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
        private readonly ICartService _cartService;


        public CartController(IGenericRepository<Item> itemRepo, IGenericRepository<Domain.Entities.Cart> cartRepo, IAuthManager authManager, ICartService cartService)
        {
            _itemRepo = itemRepo;
            _cartRepo = cartRepo;
            _authManager = authManager;
            _cartService = cartService;
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("item/add")]
        public async Task<IActionResult> AddItem([FromBody] CartItemRequestBody cartItemRequestBody)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var token = Token.GetTokenFromRequest(Request);
            
            var user = await _authManager.GetUserFromTokenAsync(token);

            if (user == null) return Unauthorized("Could not get credentials of signed in user");

            var data = await _cartService.AddCartItemAsync(cartItemRequestBody, user.Id);
            if (data == null) return StatusCode(StatusCodes.Status500InternalServerError, "Could Not Add Item To Cart");
            var res = new Response<ItemReadDto>
            {
                Status = true,
                Message = "Item Added successfully",
                Data = data,
            };

            return Created("GetItemById" , res);

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
           
            var cartItem = await _cartService.DeleteCartItemByIdAsync(itemId, user.Id);
            if(cartItem != null)
            {
                   

                var cartReadDtos = _cartRepo.Mapper.Map<ItemReadDto>(cartItem);

                var resultObject = new Response<ItemReadDto>
                {
                    Status = true,
                    Message = "Item Removed successfully",
                    Data = cartReadDtos
                };

                return Ok(resultObject);
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

            var cartItem = await _cartService.GetCartItemByIdAsync(id, user.Id);
            if(cartItem != null)
            {   
                var response = new Response<ItemReadDto>
                {
                    Status = true,
                    Message = "Item Retrieved Successfully",
                    Data = cartItem
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

            var cartItems = await _cartService.SearchCartItemsAsync(parameters, user.Id);

            if(cartItems.Count() > 0)
            {
                var response = new Response<IEnumerable<ItemReadDto>>
                {
                    Status = true,
                    Message = "Cart Items Retrieved Successfully",
                    Data = cartItems
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


