using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using BackendAPI.WebApi.Dtos;
using BackendAPI.WebApi.Models;
using BackendAPI.WebApi.Repositories;
using BackendAPI.WebApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace BackendAPI.WebApi.Controllers {
    [ApiController]
    [Route("api/world")]
    [Authorize]
    public class WorldController : ControllerBase
    {
        private readonly IWorldRepository _worldRepo;
        private readonly IObjectRepository _objectRepo;

        public WorldController(IWorldRepository worldRepo, IObjectRepository objectRepo) {
            _worldRepo = worldRepo;
            _objectRepo = objectRepo;
        }
        
        // GET /api/world -> list worlds
        [HttpGet]
        public async Task<IActionResult> GetMyWorlds() {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var worlds = await _worldRepo.GetWorldsByOwnerAsync(userId);
            return Ok(worlds);
        }
        
        // GET /api/world/{worldId} -> Get details of a single world + objects
        [HttpGet("{worldId}")]
        public async Task<IActionResult> GetWorld(int worldId) {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var world = await _worldRepo.GetWorldByIdAsync(worldId);

            if (world == null)
                return NotFound("World not found.");

            if (world.OwnerId != userId)
                return Forbid("You are not the owner of this world.");

            var objects = await _objectRepo.GetObjectsByWorldIdAsync(worldId);

            return Ok(new {
                World = world,
                Objects = objects
            });
        }
        
        // POST /api/world -> Create a new world
        [HttpPost]
        public async Task<IActionResult> CreateWorld([FromBody] CreateWorldDto dto) {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            // Limit for 5 Worlds
            int worldCount = await _worldRepo.CountWorldsByOwnerAsync(userId);
            if (worldCount >= 5)
                return BadRequest("You already have the maximum (5) worlds.");
            
            // Validate World Size
            if (dto.SizeX < 20 || dto.SizeX > 200 || dto.SizeY < 10 || dto.SizeY > 100)
                return BadRequest("World size out of valid range.");

            var newWorld = new World {
                Name = dto.Name,
                SizeX = dto.SizeX,
                SizeY = dto.SizeY,
                OwnerId = userId,
            };
            
            int newId = await _worldRepo.CreateWorldAsync(newWorld);
            return Ok(new { WorldId = newId });
        }
        
        // DELETE /api/world/{worldId} -> Delete a world
        [HttpDelete("{worldId}")]
        public async Task<IActionResult> DeleteWorld(int worldId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var world = await _worldRepo.GetWorldByIdAsync(worldId);
            
            if (world == null) return NotFound("World not found.");
            if (world.OwnerId != userId)
                return Forbid("You are not the owner of this world.");
            
            // Before world deletion, delete its objects
            await _objectRepo.DeleteObjectsByWorldIdAsync(worldId);
            
            bool deleted = await _worldRepo.DeleteWorldAsync(worldId);
            return deleted ? Ok ("World deleted.") : StatusCode(500, "Failed to delete world.");
        }
        
        // POST /api/world/{worldId}/objects -> Add objects to a world
        [HttpPost("{worldId}/objects")]
        public async Task<IActionResult> AddObjectToWorld(int worldId, [FromBody] CreateObjectDto dto) {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var world = await _worldRepo.GetWorldByIdAsync(worldId);
            
            if (world == null) return NotFound("World not found.");
            if (world.OwnerId != userId)
                return Forbid("You are not the owner of this world.");

            var obj = new WorldObject() {
                WorldId = worldId,
                Type = dto.Type,
                PositionX = dto.PositionX,
                PositionY = dto.PositionY,
                Rotation = dto.Rotation,
                Scale = dto.Scale,
            };
            
            int newObjectId = await _objectRepo.AddObjectAsync(obj);
            return Ok(new {ObjectId = newObjectId});
        }
        
        // GET /api/world/{worldId}/objects -> List objects in a world
        [HttpGet("{worldId}/objects")]
        public async Task<IActionResult> GetObjectsInWorld(int worldId) {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var world = await _worldRepo.GetWorldByIdAsync(worldId);
            
            if (world == null) return NotFound("World not found.");
            if (world.OwnerId != userId)
                return Forbid("You are not the owner of this world.");
            
            var objects = await _objectRepo.GetObjectsByWorldIdAsync(worldId);
            return Ok(objects);
        }
        
        // PUT /api/world/{worldId}/objects/{objectId} -> Update object
        [HttpPut("{worldId}/objects/{objectId}")]
        public async Task<IActionResult> UpdateObject(int worldId, int objectId, [FromBody] CreateObjectDto dto) {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var world = await _worldRepo.GetWorldByIdAsync(worldId);
            
            if (world == null) return NotFound("World not found.");
            if (world.OwnerId != userId)
                return Forbid("You are not the owner of this world.");
            
            var existingObj = await _objectRepo.GetObjectByIdAsync(objectId);
            if (existingObj == null) return NotFound("Object not found.");
            if (existingObj.WorldId != worldId) return Forbid("Object is not in this world.");
            
            // Update Object
            existingObj.Type = dto.Type;
            existingObj.PositionX = dto.PositionX;
            existingObj.PositionY = dto.PositionY;
            existingObj.Rotation = dto.Rotation;
            existingObj.Scale = dto.Scale;
            
            bool updated = await _objectRepo.UpdateObjectAsync(existingObj);
            return updated ? Ok("Object updated.") : StatusCode(500, "Failed to update object.");
        }
        
        // DELETE /api/world{worldId}/objects/{objectId}
        [HttpDelete("{worldId}/objects/{objectId}")]
        public async Task<IActionResult> DeleteObject(int worldId, int objectId) {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var world = await _worldRepo.GetWorldByIdAsync(worldId);
            
            if (world == null) return NotFound("World not found.");
            if (world.OwnerId != userId)
                return Forbid("You are not the owner of this world.");
            
            var existingObj = await _objectRepo.GetObjectByIdAsync(objectId);
            if (existingObj == null) return NotFound("Object not found.");
            if (existingObj.WorldId != worldId) return Forbid("Object is not in this world.");
            
            bool deleted = await _objectRepo.DeleteObjectAsync(objectId);
            return deleted ? Ok("Object deleted.") : StatusCode(500, "Failed to delete object.");
        }
    }
}












