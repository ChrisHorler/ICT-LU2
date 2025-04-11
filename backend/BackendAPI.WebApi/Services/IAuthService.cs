using BackendAPI.WebApi.Dtos;
using BackendAPI.WebApi.Models;

namespace BackendAPI.WebApi.Services {
    public interface IAuthService {
        Task<bool> RegisterAsync(UserDto dto);
        Task<string?> LoginAsync(UserDto dto);
    }
}