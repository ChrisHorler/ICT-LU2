using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using BackendAPI.WebApi.Models;

namespace BackendAPI.WebApi.Repositories
{
    public class ObjectRepository : IObjectRepository
    {
        private readonly IConfiguration _config;
        private IDbConnection Connection
            => new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        
        public ObjectRepository(IConfiguration config) {
            _config = config;
        }

        public async Task<int> AddObjectAsync(WorldObject obj) {
            const string sql = @"
                INSERT INTO Objects (WorldId, Type, PositionX, PositionY, Rotation, Scale)
                VALUES (@WorldId, @Type, @PositionX, @PositionY, @Rotation, @Scale)
                SELECT CAST(SCOPE_IDENTITY() as int)";
            
            using var sqlCon = Connection;
            return await sqlCon.ExecuteScalarAsync<int>(sql, obj);
        }

        public async Task<IEnumerable<WorldObject>> GetObjectsByWorldIdAsync(int worldId) {
            const string sql = "SELECT * FROM Objects WHERE WorldId = @WorldId";
            using var sqlCon = Connection;
            return await sqlCon.QueryAsync<WorldObject>(sql, new { WorldId = worldId });
        }

        public async Task<WorldObject?> GetObjectByIdAsync(int objectId) {
            const string sql = "SELECT * FROM objects WHERE Id = @ObjectId";
            using var sqlCon = Connection;
            return await sqlCon.QueryFirstOrDefaultAsync<WorldObject>(sql, new { ObjectId = objectId });
        }

        public async Task<bool> UpdateObjectAsync(WorldObject obj) {
            const string sql = @"
                UPDATE Objects
                SET Type = @Type
                    PositionX = @PositionX
                    PositionY = @PositionY
                    Rotation = @Rotation
                    Scale = @Scale
                WHERE Id = @Id";
            
            using var sqlCon = Connection;
            var rows = await sqlCon.ExecuteAsync(sql, obj);
            return rows > 0;
        }

        public async Task<bool> DeleteObjectAsync(int objectId) {
            const string sql = "DELETE FROM Objects WHERE Id = @ObjectId";
            using var sqlCon = Connection;
            var rows = await sqlCon.ExecuteAsync(sql, new { ObjectId = objectId });
            return rows > 0;
        }

        //Call before world deletion to cleanup objects of that world upon deletion
        public async Task<bool> DeleteObjectsByWorldIdAsync(int worldId) {
            const string sql = "DELETE FROM Objects WHERE WorldId = @WorldId";
            using var sqlCon = Connection;
            var rows = await sqlCon.ExecuteAsync(sql, new { WorldId = worldId });
            return rows > 0;
        }
    }
}