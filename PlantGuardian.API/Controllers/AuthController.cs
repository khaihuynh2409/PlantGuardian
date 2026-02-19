using Microsoft.AspNetCore.Mvc;
using PlantGuardian.API.DTOs;
using PlantGuardian.API.Models;
using PlantGuardian.API.Services;

namespace PlantGuardian.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegisterDto request)
        {
            if (await _authService.UserExists(request.Username))
            {
                return BadRequest("User already exists.");
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email
            };

            var registeredUser = await _authService.Register(user, request.Password);
            if (registeredUser == null) return BadRequest("Registration failed.");

            return Ok(registeredUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {
            var token = await _authService.Login(request.Username, request.Password);

            if (token == null)
            {
                return BadRequest("Wrong username or password.");
            }

            return Ok(token);
        }
    }
}
