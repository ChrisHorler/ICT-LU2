using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using BackendAPI.WebApi.Models;

namespace BackendAPI.WebApi.Repositories {
    public class WorldRespository : IWorldRepository {
        private readonly IConfiguration _config;
        private IDbConnection Connection
            => new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        
        public WorldRespository(IConfiguration config) {
            _config = config;
        }


        
        public async Task<int> CreateWorldAsync(World world) {
            const string sql = @"
                INSERT INTO Worlds (Name, OwnerId, SizeX, SizeY)
                VALUES (@Name, @OwnerId, @SizeX, @SizeY);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var sqlCon = Connection;
            return await sqlCon.ExecuteScalarAsync<int>(sql, world);
        }

        public async Task<World?> GetWorldByIdAsync(int worldId) {
            const string sql = "SELECT * FROM Worlds WHERE Id = @WorldId";
            using var sqlCon = Connection;
            return await sqlCon.QuerySingleOrDefaultAsync<World>(sql, new { WorldId = worldId });
        }

        public async Task<IEnumerable<World>> GetWorldsByOwnerAsync(int ownerId) {
            const string sql = "SELECT * FROM Worlds WHERE OwnerId = @OwnerId";
            using var sqlCon = Connection;
            return await sqlCon.QueryAsync<World>(sql, new { OwnerId = ownerId });
        }

        public async Task<int> CountWorldsByOwnerAsync(int ownerId) {
            const string sql = "SELECT COUNT(*) FROM Worlds WHERE OwnerId = @OwnerId";
            using var sqlCon = Connection;
            return await sqlCon.ExecuteScalarAsync<int>(sql, new { OwnerId = ownerId });
        }

        public async Task<bool> DeleteWorldAsync(int worldId) {
            const string sql = "DELETE FROM Worlds WHERE Id = @WorldId";
            using var sqlCon = Connection;
            var rows = await sqlCon.ExecuteAsync(sql, new { WorldId = worldId });
            return rows > 0;
        }
    }
}