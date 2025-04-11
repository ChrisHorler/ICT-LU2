using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using BackendAPI.WebApi.Dtos;
using BackendAPI.WebApi.Models;
using BackendAPI.WebApi.Repositories;

namespace BackendAPI.WebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration config, IUserRepository userRepository) {
            _config = config;
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterAsync(UserDto dto) {
            //TODO: Add Password Rules
            if (dto.Password.Length < 10) return false;
            
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null) return false;
            
            
            //TODO: Encrypt the Password in a more complex way
            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = hash
            };

            await _userRepository.AddUserAsync(user);
            return true;
        }

        public async Task<string?> LoginAsync(UserDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null) return null;
            
            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!valid) return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}