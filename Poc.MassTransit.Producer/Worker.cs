using MassTransit;
using MassTransit.MessageData.Values;
using Poc.MassTransit.Common;

namespace Poc.MassTransit.Producer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly ISendEndpointProvider sendEndpointProvider;

        public Worker(ILogger<Worker> logger, ISendEndpointProvider sendEndpointProvider)
        {
            this.logger = logger;
            this.sendEndpointProvider = sendEndpointProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 1;
            while (!stoppingToken.IsCancellationRequested && i <= 10)
            {
                var id = Guid.NewGuid();
                var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:claim-check-poc"));
                await endpoint.Send(new ClaimCheckMessage
                {
                    Id = id,
                    Index = i,
                    Content = new PutMessageData<ContentMessage>(new ContentMessage
                    {
                        Id = id,
                        Name = $"Arielle {i}"
                    })
                }, stoppingToken); ;
                i++;
            }
        }
    }
}