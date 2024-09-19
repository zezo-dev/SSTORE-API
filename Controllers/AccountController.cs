using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities.IdentityEntity;
using Store.Service.HandleResponse;
using Store.Service.Services.UserService;
using Store.Service.Services.UserService.Dto;
using System.Security.Claims;

namespace STORE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserManager<AppUser> _UserManager { get; }

        public AccountController(IUserService userService,UserManager<AppUser> userManager)
        {
           _userService = userService;
            _UserManager = userManager;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto input)
        {
            var user  = await _userService.LoginIn(input);
            if (user == null)
            {
                return Unauthorized(new CustomException(401));
            }

            return Ok(user);
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto input)
        {
            var user = await _userService.Rgister(input);
            if (user == null)
            {
                return BadRequest(new CustomException(400));
            }

            return Ok(user);
        }

        /*token eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFiZG9AZ21haWwuY29tIiwiZ2l2ZW5fbmFtZSI6ImFiZG8iLCJuYmYiOjE3MjUyODE4MzEsImV4cCI6MTcyNTI5NjIzMSwiaWF0IjoxNzI1MjgxODMxLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MjM1LyJ9.x4W9yy2DDH49QqEY2sGVqimKVJunJB9hr--bw64QdBk*/



        [HttpPost("CurrentUserDetails")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUserDetails( )
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);
            var user  =await _UserManager.FindByEmailAsync(email);

            return new UserDto
            {
                Email = user.Email,
                DisplayName= user.DisplayName,
            };

            
        }

    }
}
