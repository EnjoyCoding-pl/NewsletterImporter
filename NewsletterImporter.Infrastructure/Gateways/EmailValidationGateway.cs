using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NewsletterImporter.Core.Interfaces;
using NewsletterImporter.Domain.Models;
using NewsletterImporter.Infrastructure.Gateways.Models;

namespace NewsletterImporter.Infrastructure.Gateways
{
    public class EmailValidationGateway : IEmailValidationGateway
    {
        internal const string ClientName = "mailboxlayer";
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _accessToken;
        public EmailValidationGateway(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _accessToken = configuration.GetValue<string>("EmailValidationApi:AccessToken");
        }
        public async Task<EmailStatus> CheckAsync(string email)
        {
            var client = _clientFactory.CreateClient(ClientName);
            var response = await client.GetAsync($"check?access_key={_accessToken}&email={email}&smtp=1&format=1");

            if (!response.IsSuccessStatusCode) return null;
            var body = await response.Content.ReadAsStringAsync();
            var status = JsonSerializer.Deserialize<EmailStatusDTO>(body);

            return new EmailStatus
            {
                IsFormatValid = status.IsFormatValid,
                IsMxFound = status.IsMxFound
            };

        }
    }
}