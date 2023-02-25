namespace Poc.MassTransit.Common
{
    public class QueueTwoMessage
    {
        public Guid Id { get; private set; }
        public int Index { get; private set; }
        public string Name = "Queue two message";

        public QueueTwoMessage(Guid id, int index)
        {
            Id = id;
            Index = index;
        }
    }
}
