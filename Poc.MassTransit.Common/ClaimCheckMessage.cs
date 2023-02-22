using MassTransit;

namespace Poc.MassTransit.Common
{
    public class ClaimCheckMessage
    {
        public Guid Id { get; set; }
        public int Index { get;set; }
        public MessageData<ContentMessage>? Content { get; set; }

    }

    public class ContentMessage
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}