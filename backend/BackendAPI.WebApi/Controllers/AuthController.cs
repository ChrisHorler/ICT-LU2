using Microsoft.AspNetCore.Mvc;
using BackendAPI.WebApi.Dtos;
using BackendAPI.WebApi.Models;
using BackendAPI.WebApi.Services;

namespace BackendAPI.WebApi.Controllers {
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto dto) {
            var success = await _authService.RegisterAsync(dto);
            return success ? Ok(success) : BadRequest("Registration failed (username exists or invalid password).");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto dto) {
            var token = await _authService.LoginAsync(dto);
            return token != null ? Ok(token) : BadRequest("Invalid Credentials.");
        }
    }
}