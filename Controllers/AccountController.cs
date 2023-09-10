using AutoMapper;
using CartApi.Domain.Entities;
using CartApi.Interfaces;
using HotelApi.DTOS.WriteDtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CartAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthManager _authManager;
        public AccountController(
            ILogger<AccountController> logger,
            IMapper mapper,
            SignInManager<ApiUser> signInManager,
            UserManager<ApiUser> userManager,
            IAuthManager authManager)
        {
            _logger = logger;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _authManager = authManager;
        }

        [HttpPost("Register", Name = "Register")]
        public async Task<IActionResult> Register([FromBody] UserWriteDto userWriteDto)
        {
            var user = _mapper.Map<ApiUser>(userWriteDto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            var result = await _userManager.CreateAsync(user, userWriteDto.Password);
            if (result.Succeeded)
            {
                return StatusCode(201, "User Created Successfully");

            }
            else
            {
                return BadRequest(result.Errors);
            }

        }

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("Login", Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginWriteDto loginInfo)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUserValidated = await _authManager.AuthenticateUserAsync(loginInfo);
            if (isUserValidated)
            {
                var token = await _authManager.GenerateTokenAsync(loginInfo.UserName);
                return Accepted(new { Token = token });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
