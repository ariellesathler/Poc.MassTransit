using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MassTransit;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Poc.MassTransit.AzureFunctions.Consumers;

namespace Poc.MassTransit.AzureFunctions
{
    public class ClaimCheckFunction
    {
        readonly IMessageReceiver receiver;

        public ClaimCheckFunction(IMessageReceiver receiver)
        {
            this.receiver = receiver;
        }

        [FunctionName("ClaimCheckFunction")]
        public async Task Run([ServiceBusTrigger("claim-check-poc")] ServiceBusReceivedMessage message, CancellationToken cancellationToken)
        {
            await receiver.HandleConsumer<MessageConsumer>("claim-check-poc", message, cancellationToken); 
        }
    }
}
