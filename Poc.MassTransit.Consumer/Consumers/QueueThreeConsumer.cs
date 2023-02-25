using MassTransit;
using Poc.MassTransit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.MassTransit.Consumer.Consumers
{
    internal class QueueThreeConsumer : IConsumer<QueueThreeMessage>
    {
        readonly ILogger<QueueThreeConsumer> logger;

        public QueueThreeConsumer(ILogger<QueueThreeConsumer> logger)
        {
            this.logger = logger;
        }

        public Task Consume(ConsumeContext<QueueThreeMessage> context)
        {
            Console.WriteLine($"Processando mensagem fila 3 - id {context.MessageId}");

            return Task.CompletedTask;
        }
    }
}
