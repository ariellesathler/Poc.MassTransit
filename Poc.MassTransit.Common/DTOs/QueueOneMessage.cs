using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.MassTransit.Common
{
    public class QueueOneMessage
    {
        public Guid Id { get; private set; }
        public int Index { get; private set; }
        public string Name = "Queue one message";

        public QueueOneMessage(Guid id, int index)
        {
            Id = id;
            Index = index;
        }
    }

    public class QueueTwoWrongObject
    {
        public Guid Id { get; private set; }
        public Guid ExternalId { get; private set; }

        public string Context = "Unespected message format";

        public QueueTwoWrongObject(Guid id, Guid externalId)
        {
            Id = id;
            ExternalId = externalId;
        }
    }
}
