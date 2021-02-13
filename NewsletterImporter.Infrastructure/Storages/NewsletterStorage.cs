using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using NewsletterImporter.Core.Interfaces;
using NewsletterImporter.Domain.Models;
using NewsletterImporter.Infrastructure.Storages.Models;

namespace NewsletterImporter.Infrastructure.Storages
{
    public class NewsletterStorage : INewsletterStorage
    {
        private readonly IConfiguration _configuration;

        public NewsletterStorage(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddUsersAsync(List<(User User, EmailStatus Status)> users)
        {
            var client = new MongoClient(_configuration.GetConnectionString("Mongo"));
            var database = client.GetDatabase("Newsletter");
            var collection = database.GetCollection<NewsLetterUser>("Users");

            await collection.InsertManyAsync(users.Select(x => new NewsLetterUser
            {
                Email = x.User.Email,
                CreatDate = DateTimeOffset.UtcNow
            }));
        }

        public async IAsyncEnumerable<User> GetUsersAsync()
        {
            var client = new MongoClient(_configuration.GetConnectionString("Mongo"));
            var database = client.GetDatabase("Newsletter");
            var collection = database.GetCollection<NewsLetterUser>("Users");

            var options = new FindOptions<NewsLetterUser>
            {
                Sort = Builders<NewsLetterUser>.Sort.Ascending(x => x.Email)
            };

            var cursor = await collection.FindAsync(Builders<NewsLetterUser>.Filter.Empty, options);

            while (await cursor.MoveNextAsync())
            {
                foreach (var item in cursor.Current)
                {
                    yield return new User
                    {
                        Email = item.Email
                    };
                }
            }
        }

        public Task DeleteUsersAsync(List<User> users)
        {
            var client = new MongoClient(_configuration.GetConnectionString("Mongo"));
            var database = client.GetDatabase("Newsletter");
            var collection = database.GetCollection<NewsLetterUser>("Users");
            var filter = Builders<NewsLetterUser>.Filter.In(x => x.Email, users.Select(x => x.Email));

            return collection.DeleteManyAsync(filter);
        }
    }
}