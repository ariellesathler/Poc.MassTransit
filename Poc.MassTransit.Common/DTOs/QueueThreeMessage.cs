namespace Poc.MassTransit.Common
{
    public class QueueThreeMessage
    {
        public Guid Id { get; private set; }
        public int Index { get; private set; }
        public string Name = "Queue three message";

        public QueueThreeMessage(Guid id, int index)
        {
            Id = id;
            Index = index;
        }
    }
}
