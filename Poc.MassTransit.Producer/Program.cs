using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using MassTransit;
using Poc.MassTransit.Common.Config;
using Poc.MassTransit.Producer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var config = context.Configuration.Get<AppConfig>();

        //services.AddHostedService<ClaimCheckProducer>();
        //services.AddHostedService<QueueOneProducer>();
        //services.AddHostedService<QueueTwoProducer>();
        //services.AddHostedService<QueueThreeProducer>();
        services.AddHostedService<BatchMessageProducer>();

        var options = new AWSOptions
        {
            Credentials = new BasicAWSCredentials(config.AWS!.AccessKeyId, config.AWS.SecretAccessKey),
            Region = Amazon.RegionEndpoint.USEast1,
            DefaultConfigurationMode = DefaultConfigurationMode.Standard
        };

        //Informar somente para desenvolvimento Local utilizando LocalStack
        options.DefaultClientConfig.ServiceURL = config.AWS!.ServiceURL;


        services.AddAWSService<IAmazonS3>(options);

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
                    MessageDataDefaults.Threshold = 230000;
                    MessageDataDefaults.AlwaysWriteToRepository = false;
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
