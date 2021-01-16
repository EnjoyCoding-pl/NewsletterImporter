using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NewsletterImporter.Domain.Models;

namespace NewsletterImporter.Core.Abstract
{
    public interface IUserRepository
    {
        IAsyncEnumerable<User> GetSignedUsersAsync();
    }
}