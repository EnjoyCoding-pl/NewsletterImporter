using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NewsletterImporter.Core.Abstract;
using NewsletterImporter.Infrastructure.Gateways;
using NewsletterImporter.Infrastructure.Repositories;
using Polly;
using System.Net.Http;
using Polly.Extensions.Http;
using System.Net;
using NewsletterImporter.Core.Interfaces;
using NewsletterImporter.Infrastructure.Storages;
using Microsoft.Extensions.Configuration;

namespace NewsletterImporter.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNewsletterImporterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IEmailValidationGateway,EmailValidationGateway>();
            services.AddTransient<INewsletterStorage,NewsletterStorage>();
            services.AddHttpClient(EmailValidationGateway.ClientName, config =>
            {
                config.BaseAddress = new Uri(configuration.GetValue<string>("EmailValidationApi:Uri"));
            })
            .AddPolicyHandler(GetPolicy());
        }

        public static IAsyncPolicy<HttpResponseMessage> GetPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError()
            .OrResult(result => result.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}