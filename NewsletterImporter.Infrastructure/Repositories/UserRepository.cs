using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using NewsletterImporter.Domain.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using NewsletterImporter.Core.Interfaces;

namespace NewsletterImporter.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async IAsyncEnumerable<User> GetSignedUsersAsync()
        {
            var connectionString = _configuration.GetConnectionString("SqlServer");
            await using (var connection = new SqlConnection(connectionString))
            {
                var reader = await connection.ExecuteReaderAsync(new CommandDefinition(@"
                    SELECT Email FROM [dbo].[Users] WHERE Newsletter = 1 ORDER BY Email"));

                var parser = reader.GetRowParser<User>();

                while (await reader.ReadAsync())
                {
                    yield return parser(reader);
                }

            }
        }
    }
}