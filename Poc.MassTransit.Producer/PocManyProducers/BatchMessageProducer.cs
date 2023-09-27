using MassTransit;
using Poc.MassTransit.Common;

namespace Poc.MassTransit.Producer
{
    public class BatchMessageProducer : BackgroundService
    {
        private readonly ISendEndpointProvider sendEndpointProvider;

        public BatchMessageProducer(ISendEndpointProvider sendEndpointProvider)
        {
            this.sendEndpointProvider = sendEndpointProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var messages = new List<QueueOneMessage>();
            for (int i = 0; i < 10; i++)
            {
                messages.Add(new QueueOneMessage(Guid.NewGuid(), i));
            }

            var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:queue-one"));
            await endpoint.SendBatch(messages, stoppingToken);
        }
    }
}