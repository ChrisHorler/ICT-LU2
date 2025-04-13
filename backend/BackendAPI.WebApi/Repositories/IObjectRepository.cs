using BackendAPI.WebApi.Models;

namespace BackendAPI.WebApi.Repositories
{
    public interface IObjectRepository
    {
        Task<int> AddObjectAsync(WorldObject obj);
        Task<IEnumerable<WorldObject>> GetObjectsByWorldIdAsync(int worldId);
        Task<WorldObject?> GetObjectByIdAsync(int objectId);
        
        Task<bool> UpdateObjectAsync(WorldObject obj);
        Task<bool> DeleteObjectAsync(int objectId);
        Task<bool> DeleteObjectsByWorldIdAsync(int worldId);
        
    }
}