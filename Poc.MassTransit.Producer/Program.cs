using Azure.Storage.Blobs;
using MassTransit;
using MassTransit.MongoDbIntegration.MessageData;
using Poc.MassTransit.Producer;

string blobConnection = "";
string containerName = "";
string serviceBusConnection = "";

//Claim check com MongoDb
string mongoConnection = "mongodb://root:x7j0mH8M%28cBo@127.0.0.1:27017/claim-check?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false";
string mongoDatabase = "claim-check";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var messageDataRepository = new BlobServiceClient(blobConnection)
                .CreateMessageDataRepository(containerName);

        //Claim check com MongoDb
        //var messageDataRepository = new MongoDbMessageDataRepository(mongoConnection, mongoDatabase);

        services.AddHostedService<Worker>();

        services.AddMassTransit(x =>
        {
            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(serviceBusConnection);
                cfg.ConfigureEndpoints(context);
                cfg.UseMessageData(messageDataRepository);
            });
        });

        services.AddSingleton<ISendEndpointProvider>(p => p.GetRequiredService<IBusControl>());
    })
    .Build();

await host.RunAsync();


