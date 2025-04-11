using BackendAPI.WebApi.Models;

namespace BackendAPI.WebApi.Repositories {
    public interface IUserRepository {
        Task<User?> GetByUsernameAsync(string username);
        Task<int> AddUserAsync(User user);
    }
}