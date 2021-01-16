using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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