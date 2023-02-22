using MassTransit;
using Poc.MassTransit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.MassTransit.AzureFunctions.Consumers
{
    public class MessageConsumer : IConsumer<ClaimCheckMessage>
    {
        public async Task Consume(ConsumeContext<ClaimCheckMessage> context)
        {
            var message = await context.Message.Content.Value;

            //throw new NotImplementedException();
        }
    }
}
