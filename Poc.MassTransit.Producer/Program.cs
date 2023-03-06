using Amazon.S3;
using MassTransit;
using Poc.MassTransit.Common.Config;
using Poc.MassTransit.Common;
using Poc.MassTransit.Producer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration.Get<AppConfig>();

        services.AddHostedService<ClaimCheckProducer>();
        //services.AddHostedService<QueueOneProducer>();
        //services.AddHostedService<QueueTwoProducer>();
        //services.AddHostedService<QueueThreeProducer>();
        //services.AddHostedService<PocS3>();

        services.AddAWSService<IAmazonS3>();
        services.AddDefaultAWSOptions(context.Configuration.GetAWSOptions());
        services.AddSingleton<ISendEndpointProvider>(p => p.GetRequiredService<IBusControl>());

        var provider = services.BuildServiceProvider();

        services.AddMassTransit(x =>
        {
            if (config.PocKind!.BusType.Equals("ServiceBus"))
            {
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(config.Bus!.ConnectionString);
                    cfg.ConfigureEndpoints(context);
                    cfg.UseMessageData(MessageDataRepositoryFactory.GetRepository(config, provider));
                    // MessageDataDefaults.Threshold = 23000;
                    //MessageDataDefaults.AlwaysWriteToRepository = false;
                    //MessageDataDefaults.TimeToLive = TimeSpan.FromDays(30);
                });
            }
            else
            {
                x.UsingRabbitMq((context, cfg) =>
               {
                   cfg.Host(config.Bus!.ConnectionString);
                   cfg.ConfigureEndpoints(context);
                   cfg.UseMessageData(MessageDataRepositoryFactory.GetRepository(config, provider));
               });
            }
        });

    })
    .Build();

await host.RunAsync();
