using MassTransit;
using MassTransit.MessageData.Values;
using Poc.MassTransit.Common;

namespace Poc.MassTransit.Producer
{
    public class QueueThreeProducer : BackgroundService
    {
        private readonly ILogger<QueueThreeProducer> logger;
        private readonly ISendEndpointProvider sendEndpointProvider;

        public QueueThreeProducer(ILogger<QueueThreeProducer> logger, ISendEndpointProvider sendEndpointProvider)
        {
            this.logger = logger;
            this.sendEndpointProvider = sendEndpointProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 1;
            while (!stoppingToken.IsCancellationRequested && i <= 10)
            {
                var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:queue-three"));
                await endpoint.Send(new QueueThreeMessage(Guid.NewGuid(), i), stoppingToken);
                i++;
            }
        }
    }
}