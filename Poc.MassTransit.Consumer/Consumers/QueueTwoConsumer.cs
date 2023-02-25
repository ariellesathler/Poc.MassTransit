using MassTransit;
using Poc.MassTransit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.MassTransit.Consumer.Consumers
{
    internal class QueueTwoConsumer : IConsumer<QueueTwoMessage>
    {
        readonly ILogger<QueueTwoConsumer> logger;

        public QueueTwoConsumer(ILogger<QueueTwoConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<QueueTwoMessage> context)
        {
            Console.WriteLine($"Processando mensagem fila 2 - id {context.MessageId}");

            return Task.CompletedTask;
        }
    }
}
