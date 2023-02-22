using Azure.Storage.Blobs;
using MassTransit;
using MassTransit.MongoDbIntegration.MessageData;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Poc.MassTransit.AzureFunctions;
using Poc.MassTransit.AzureFunctions.Consumers;
using Poc.MassTransit.Common;
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
        private string blobConnection = "";
        private string containerName = "";

        //Claim check com MongoDB
        string mongoConnection = "mongodb://root:x7j0mH8M%28cBo@127.0.0.1:27017/claim-check?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false";
        string mongoDatabase = "claim-check";

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var messageDataRepository = new BlobServiceClient(blobConnection)
                .CreateMessageDataRepository(containerName);

            //var messageDataRepository = new MongoDbMessageDataRepository(mongoConnection, mongoDatabase);

            builder.Services
                .AddScoped<ClaimCheckFunction>()
                .AddMassTransitForAzureFunctions(cfg =>
                {
                    //cfg.AddConsumersFromNamespaceContaining<MessageConsumer>();
                    cfg.AddConsumer<MessageConsumer>();
                    cfg.AddRequestClient<ClaimCheckMessage>(new Uri("queue:claim-check-poc"));

                }, connectionStringConfigurationKey: "AzureWebJobsServiceBus", (context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.UseMessageData(messageDataRepository);
                    //cfg.PrefetchCount = 1;
                    //cfg.MaxConcurrentCalls = 1;
                });
        }
    }
}
