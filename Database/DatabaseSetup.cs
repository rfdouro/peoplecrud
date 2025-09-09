using Microsoft.Data.Sqlite;

namespace PeopleCrud.Database;

public static class DatabaseSetup
{
    public static void Initialize(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS People (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Age INTEGER NOT NULL,
                Email TEXT,
                CreatedAt DATETIME NOT NULL
            );
        ";
        
        command.ExecuteNonQuery();
    }
}