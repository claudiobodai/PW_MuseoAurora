using Dapper;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using Models;

namespace PW_Museo.Repositories
{
    public class ExhibitionsRepository
    {
        private readonly string _connectionString;

        public ExhibitionsRepository(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:db"] ?? throw new InvalidOperationException("Connection string 'db' not found.");
        }

        public async Task<IEnumerable<Show>> GetAllAsync()
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryAsync<Show>("SELECT * FROM Shows");
        }

        public async Task<Show> GetByIdAsync(Guid id)
        {
            using var connection = new MySqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Show>("SELECT * FROM Shows WHERE Id = @Id", new { Id = id });
        }

        public async Task CreateAsync(Show show)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "INSERT INTO Shows (Id, Title, Description, StartDate, EndDate, Status, AuthorId) VALUES (@Id, @Title, @Description, @StartDate, @EndDate, @Status, @AuthorId)";
            await connection.ExecuteAsync(sql, show);
        }

        public async Task UpdateAsync(Show show)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "UPDATE Shows SET Title = @Title, Description = @Description, StartDate = @StartDate, EndDate = @EndDate, Status = @Status, AuthorId = @AuthorId WHERE Id = @Id";
            await connection.ExecuteAsync(sql, show);
        }

        public async Task DeleteAsync(Guid id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "DELETE FROM Shows WHERE Id = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
