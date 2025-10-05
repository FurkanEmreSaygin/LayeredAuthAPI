using Business.DTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto model)
        {
            try
            {
                var response = await _userService.RegisterUserAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            try
            {
                var token = await _userService.LoginUserAsync(model);
                return Ok(new { Token = token });
            }
            catch (Exception)
            {
                return Unauthorized(new { Message = "Invalid credentials." });
            }
        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { Message = "Token does not contain a valid user ID." });
            }

            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }

        [HttpGet("admin-test")]
        [Authorize(Roles = "Admin")] 
        public IActionResult AdminTest()
        {
            return Ok(new { Message = "Welcome, Admin!" });
        }
    }
}