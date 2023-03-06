using MassTransit;
using MassTransit.MessageData.Values;
using Poc.MassTransit.Common;

namespace Poc.MassTransit.Producer
{
    public class ClaimCheckProducer : BackgroundService
    {
        private readonly ILogger<ClaimCheckProducer> logger;
        private readonly ISendEndpointProvider sendEndpointProvider;

        public ClaimCheckProducer(ILogger<ClaimCheckProducer> logger, ISendEndpointProvider sendEndpointProvider)
        {
            this.logger = logger;
            this.sendEndpointProvider = sendEndpointProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 1;
            var data = File.ReadAllText(@"C:\temp\PayloadCreateQuoteUnicoMoreThan100Risks (1).json");

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
                        Name = $"Arielle {i}",
                        Data = data
                    })
                }, stoppingToken); 
                i++;
            }
        }
    }
}