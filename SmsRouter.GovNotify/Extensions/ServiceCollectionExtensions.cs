using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmsRouter.Core;
using SmsRouter.GovNotify.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmsRouter.GovNotify.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGovNotifySvcHttpClient(this IServiceCollection services, Action<GovNotifyOptions> configLambda = null)
        {
            if (configLambda != null)
                services.Configure(configLambda);

            services.AddHttpClient(nameof(GovNotifySvc))
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var options = sp.GetRequiredService<IOptions<GovNotifyOptions>>()
                        .Value;

                    var handler = new HttpClientHandler();
                    if (options.IsProxyRequired)
                    {
                        var proxy = new WebProxy(options.ProxyUrl);
                        handler.Proxy = proxy;

                        if (!string.IsNullOrWhiteSpace(options.ProxyUsername))
                        {
                            proxy.Credentials = new NetworkCredential(options.ProxyUsername, options.ProxyPassword);
                        }
                    }

                    return handler;
                });
            services.AddTransient<ISmsSvc, GovNotifySvc>();
            return services;
        }
    }
}
