using Microsoft.Extensions.DependencyInjection;

namespace NewsletterImporter.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddNewsletterImporterCore(this IServiceCollection services)
        {
            services.AddTransient<UserImporter>();
        }
    }
}