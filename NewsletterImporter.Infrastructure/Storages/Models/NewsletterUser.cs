using System;

namespace NewsletterImporter.Infrastructure.Storages.Models
{
    public class NewsLetterUser
    {
        public string Email { get; set; }
        public DateTimeOffset CreatDate { get; set; }
    }
}