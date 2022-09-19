using System;
using Npgsql;
using NotificationService.Repository;
using NotificationService;
using NotificationService.Model;

namespace AuthService.IntegrationTests
{
    public static class DbExtensions
    {

        public static long CountTableRows(this IntegrationWebApplicationFactory<Program, AppDbContext> factory,
            string schemaName, string tableName)
        {
            long totalRows = -1;
            using (var connection = new NpgsqlConnection(factory.postgresContainer.ConnectionString))
            {
                using (var command = new NpgsqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT COUNT(*) FROM " + schemaName + ".\"" + tableName + "\"";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            totalRows = (long)reader[0];
                        }
                    }
                }
            }
            return totalRows;
        }

        public static void Insert(this IntegrationWebApplicationFactory<Program, AppDbContext> factory,
            string schemaName, string tableName, NotificationConfig config)
        {
            string insertQuery = "INSERT INTO " + schemaName + ".\"" + tableName +
                                 "\" (\"Id\", \"Messages\", \"Connections\", \"Posts\", \"ProfileId\") " +
                                 "VALUES (@Id, @Messages, @Connections, @Posts, @ProfileId)";
            using (var connection = new NpgsqlConnection(factory.postgresContainer.ConnectionString))
            {
                using (var command = new NpgsqlCommand(insertQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Id", config.Id);
                    command.Parameters.AddWithValue("@Messages", config.Messages);
                    command.Parameters.AddWithValue("@Connections", config.Connections);
                    command.Parameters.AddWithValue("@Posts", config.Posts);
                    command.Parameters.AddWithValue("@ProfileId", config.ProfileId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteById(this IntegrationWebApplicationFactory<Program, AppDbContext> factory,
            string schemaName, string tableName, Guid id)
        {
            using (var connection = new NpgsqlConnection(factory.postgresContainer.ConnectionString))
            {
                using (var command = new NpgsqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM " + schemaName + ".\"" + tableName + "\" WHERE \"Id\" = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
