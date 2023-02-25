
using MassTransit;
using Poc.MassTransit.Common.Config;
using Poc.MassTransit.Consumer.Consumers;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration.Get<AppConfig>();

        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.AddConsumer<QueueOneConsumer>();
            cfg.AddConsumer<QueueTwoConsumer>();
            cfg.AddConsumer<QueueThreeConsumer>();

            cfg.UsingRabbitMq((context, rabbitConfig) =>
            {
                rabbitConfig.Host(config.Bus!.ConnectionString);
                rabbitConfig.ConfigureEndpoints(context);
            });
        });
    })
    .Build();

await host.RunAsync();
