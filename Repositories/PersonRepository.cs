using Dapper;
using Microsoft.Data.Sqlite;
using PeopleCrud.Models;

namespace PeopleCrud.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly string _connectionString;

    public PersonRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Data Source=people.db";
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        return await connection.QueryAsync<Person>("SELECT * FROM People");
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<Person>(
            "SELECT * FROM People WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(Person person)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"INSERT INTO People (Name, Age, Email, CreatedAt) 
                   VALUES (@Name, @Age, @Email, @CreatedAt);
                   SELECT last_insert_rowid();";
        
        person.CreatedAt = DateTime.UtcNow;
        return await connection.ExecuteScalarAsync<int>(sql, person);
    }

    public async Task<bool> UpdateAsync(Person person)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"UPDATE People SET Name = @Name, Age = @Age, Email = @Email 
                   WHERE Id = @Id";
        
        var affectedRows = await connection.ExecuteAsync(sql, person);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var affectedRows = await connection.ExecuteAsync(
            "DELETE FROM People WHERE Id = @Id", new { Id = id });
        return affectedRows > 0;
    }
}