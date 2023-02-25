using MassTransit;
using MassTransit.MessageData.Values;
using Poc.MassTransit.Common;

namespace Poc.MassTransit.Producer
{
    public class QueueTwoProducer : BackgroundService
    {
        private readonly ILogger<QueueTwoProducer> logger;
        private readonly ISendEndpointProvider sendEndpointProvider;

        public QueueTwoProducer(ILogger<QueueTwoProducer> logger, ISendEndpointProvider sendEndpointProvider)
        {
            this.logger = logger;
            this.sendEndpointProvider = sendEndpointProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 1;
            //Essa fila dará erro em toda as msgs pois posta um objeto diferente do que o consumer precisa
            while (!stoppingToken.IsCancellationRequested && i <= 5)
            {
                var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:queue-two"));
                await endpoint.Send(new QueueTwoWrongObject(Guid.NewGuid(), Guid.NewGuid()), stoppingToken);
                i++;
            }
        }
    }
}