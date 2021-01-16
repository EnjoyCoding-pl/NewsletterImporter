using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsletterImporter.Domain.Models;

namespace NewsletterImporter.Core.Interfaces
{
    public interface INewsletterStorage
    {
        Task AddUsersAsync(List<(User User, EmailStatus Status)> users);
        IAsyncEnumerable<User> GetUsersAsync();
        Task DeleteUsersAsync(List<User> users);
    }
}