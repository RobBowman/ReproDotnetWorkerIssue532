using Issue532;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SmsRouter.EntityFramework;
using SmsRouter.GovNotify.Extensions;
using SmsRouter.Utrn.Extensions;
using System;
using System.IO;

namespace SmsRouter.AzFunc
{
    class Program
    {
        static void Main(string[] args)
        {
            string methodName = nameof(Main);

            try
            {
                Log.Information("Starting up! {MethodName}", methodName);

                var host = new HostBuilder()
                .UseEnvironment(Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production")
                .ConfigureHostConfiguration(config =>
                {
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .UseSerilog()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "local.settings.json"));
                        //builder.AddUserSecrets<Program>();
                    }
                })
                .ConfigureServices(s =>
                {
                    var configuration = s.BuildServiceProvider()
                        .GetRequiredService<IConfiguration>();

                    //s.AddUtrnServices(configuration.GetSection("SmsRouter:Utrn").Bind);
                    //s.AddGovNotifySvcHttpClient(configuration.GetSection("SmsRouter:GovNotify").Bind);
                    s.AddDbContext<SmsOrderContext>(options =>
                    {
                        options.UseSqlServer(configuration.GetConnectionString("SmsRouter"));
                    });

                    //s.AddSwaggerDocument();

                })
                .Build();
                //var context = host.Services.GetRequiredService<SmsOrderContext>();
                //var loggingConfig = new LoggingConfig();
                //var configuration = host.Services.GetRequiredService<IConfiguration>();
                //loggingConfig.Configure(configuration.GetConnectionString("SmsRouter"));

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
        }

    }
}
