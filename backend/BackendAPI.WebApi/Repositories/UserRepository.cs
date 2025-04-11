using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using BackendAPI.WebApi.Models;

namespace BackendAPI.WebApi.Repositories {
    public class UserRepository : IUserRepository {
        private readonly IConfiguration _config;

        public UserRepository(IConfiguration config) {
            _config = config;
        }

        private IDbConnection Connection
            => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        public async Task<User?> GetByUsernameAsync(string username) {
            const string sql = "SELECT * FROM Users WHERE Username = @Username";
            using var conn = Connection;
            return await conn.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<int> AddUserAsync(User user) {
            const string sql = @"
                INSERT INTO Users (Username, PasswordHash)
                VALUES (@Username, @PasswordHash);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var conn = Connection;
            return await conn.ExecuteScalarAsync<int>(sql, user);
        }
    }
}