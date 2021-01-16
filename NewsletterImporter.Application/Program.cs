using System;
using System.Threading.Tasks;
using Cocona;
using Cocona.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewsletterImporter.Core;
using NewsletterImporter.Infrastructure;

namespace NewsletterImporter.Application
{
    public class Program : CoconaConsoleAppBase
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices);

            var app = new CoconaAppHostBuilder(hostBuilder);

            await app.RunAsync<Program>(args);

        }
        public async Task ImportUsers([FromService] UserImporter importer)
        {
            await importer.ImportUsers();
        }

        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddNewsletterImporterCore();
            services.AddNewsletterImporterInfrastructure(context.Configuration);
        }
    }
}
