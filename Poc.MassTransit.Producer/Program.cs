using MassTransit;
using Poc.MassTransit.Common.Config;
using Poc.MassTransit.Producer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration.Get<AppConfig>();

        //services.AddHostedService<ClaimCheckProducer>();
        services.AddHostedService<QueueOneProducer>();
        services.AddHostedService<QueueTwoProducer>();
        services.AddHostedService<QueueThreeProducer>();

        services.AddMassTransit(x =>
        {
            if (config.PocKind!.BusType.Equals("ServiceBus"))
            {
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(config.Bus!.ConnectionString);
                    cfg.ConfigureEndpoints(context);
                    cfg.UseMessageData(MessageDataRepositoryFactory.GetRepository(config));
                    // MessageDataDefaults.Threshold = 23000;
                    //MessageDataDefaults.AlwaysWriteToRepository = false;
                    MessageDataDefaults.TimeToLive = TimeSpan.FromDays(30);
                });
            }
            else
            {
                x.UsingRabbitMq((context, cfg) =>
               {
                   cfg.Host(config.Bus!.ConnectionString);
                   cfg.ConfigureEndpoints(context);
                   cfg.UseMessageData(MessageDataRepositoryFactory.GetRepository(config));
               });
            }
        });

        services.AddSingleton<ISendEndpointProvider>(p => p.GetRequiredService<IBusControl>());
    })
    .Build();

await host.RunAsync();
