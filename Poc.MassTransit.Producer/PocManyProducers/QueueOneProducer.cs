using MassTransit;
using MassTransit.MessageData.Values;
using Poc.MassTransit.Common;

namespace Poc.MassTransit.Producer
{
    public class QueueOneProducer : BackgroundService
    {
        private readonly ILogger<QueueOneProducer> logger;
        private readonly ISendEndpointProvider sendEndpointProvider;

        public QueueOneProducer(ILogger<QueueOneProducer> logger, ISendEndpointProvider sendEndpointProvider)
        {
            this.logger = logger;
            this.sendEndpointProvider = sendEndpointProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 1;
            while (!stoppingToken.IsCancellationRequested && i <= 10)
            {
                var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:queue-one"));
                await endpoint.Send(new QueueOneMessage(Guid.NewGuid(), i), stoppingToken);
                i++;
            }

        }
    }
}