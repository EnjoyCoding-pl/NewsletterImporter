using System;
using MongoDB.Bson;

namespace NewsletterImporter.Infrastructure.Storages.Models
{
    public class NewsLetterUser
    {
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatDate { get; set; }
    }
}