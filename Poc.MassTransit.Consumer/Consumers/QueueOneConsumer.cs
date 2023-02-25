using MassTransit;
using Poc.MassTransit.Common;

namespace Poc.MassTransit.Consumer.Consumers
{
    internal class QueueOneConsumer : IConsumer<QueueOneMessage>
    {
        readonly ILogger<QueueOneConsumer> logger;

        public QueueOneConsumer(ILogger<QueueOneConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<QueueOneMessage> context)
        {
            Console.WriteLine($"Processando mensagem fila 1 - id {context.MessageId}");

            return Task.CompletedTask;
        }
    }
}
