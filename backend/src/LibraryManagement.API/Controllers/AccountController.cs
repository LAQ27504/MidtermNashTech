using LibraryManagement.Core.Application.DTOs.Requests;
using LibraryManagement.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest userRegister)
        {
            var res = await _userService.AddUserExecute(userRegister);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLogin)
        {
            var res = await _userService.LoginUserExecute(userLogin);
            if (!res.Success)
            {
                return Unauthorized(res.Message);
            }
            return Ok(new { res.Data, res.Message });
        }
    }
}
