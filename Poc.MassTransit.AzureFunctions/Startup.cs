using Azure.Storage.Blobs;
using MassTransit;
using MassTransit.MongoDbIntegration.MessageData;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poc.MassTransit.AzureFunctions;
using Poc.MassTransit.AzureFunctions.Consumers;
using Poc.MassTransit.Common;
using Poc.MassTransit.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Poc.MassTransit.AzureFunctions
{
    public class Startup :
       FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.GetContext().Configuration.Get<AppConfig>();

            builder.Services
                .AddScoped<ClaimCheckFunction>()
                .AddMassTransitForAzureFunctions(cfg =>
                {
                    cfg.AddConsumer<MessageConsumer>();

                }, connectionStringConfigurationKey: "AzureWebJobsServiceBus", (context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.UseMessageData(MessageDataRepositoryFactory.GetRepository(config));
                    MessageDataDefaults.Threshold = 23000;
                    MessageDataDefaults.AlwaysWriteToRepository = false;
                    MessageDataDefaults.TimeToLive = TimeSpan.FromDays(30);

                });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var context = builder.GetContext();
            var environmentName = context.EnvironmentName;

            builder.ConfigurationBuilder
                .SetBasePath(context.ApplicationRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false);
        }
    }
}
