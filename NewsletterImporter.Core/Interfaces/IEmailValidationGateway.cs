using System.Threading.Tasks;
using NewsletterImporter.Domain.Models;

namespace NewsletterImporter.Core.Interfaces
{
    public interface IEmailValidationGateway
    {
        Task<EmailStatus> CheckAsync(string email);
    }
}