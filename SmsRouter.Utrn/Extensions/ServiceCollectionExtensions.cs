using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmsRouter.Core;
using SmsRouter.Utrn.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmsRouter.Utrn.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUtrnServices(this IServiceCollection services, Action<UtrnOptions> configureAction)
        {
            if (configureAction != null)
                services.Configure(configureAction);

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<UtrnOptions>>().Value;

            services.AddHttpClient(nameof(UtrnSvc), x =>
            {
                x.BaseAddress = new Uri(options.BaseAddress);
                x.DefaultRequestHeaders.Add("x-functions-key", options.FunctionsKey);
                x.Timeout = TimeSpan.FromSeconds(60);
            });

            services.AddTransient<IUtrnSvc, UtrnSvc>();
            return services;
        }
    }
}
