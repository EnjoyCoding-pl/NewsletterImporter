using System.Collections.Generic;
using NewsletterImporter.Domain.Models;

namespace NewsletterImporter.Core.Interfaces
{
    public interface IUserRepository
    {
        IAsyncEnumerable<User> GetSignedUsersAsync();
    }
}