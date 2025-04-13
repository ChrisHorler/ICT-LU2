using BackendAPI.WebApi.Models;

namespace BackendAPI.WebApi.Repositories {
    public interface IWorldRepository {
        Task<int> CreateWorldAsync(World world);
        Task<World?> GetWorldByIdAsync(int worldId);
        Task<IEnumerable<World>> GetWorldsByOwnerAsync(int ownerId);
        Task<int> CountWorldsByOwnerAsync(int ownerId);
        Task<bool> DeleteWorldAsync(int worldId);
        
        Task<bool> WorldNameExistsForOwnerAsync(int ownerId, string name);
    }
}