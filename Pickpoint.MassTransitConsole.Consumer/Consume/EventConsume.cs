using Common;
using MassTransit;

namespace Pickpoint.MassTransitConsole.Consumer.Consume
{
    class EventConsumer : IConsumer<SendMessage>
    {
        public static int MessageCount
        {
            get
            {
                return messageCount;
            }
        }

        private static int messageCount;

        public Task Consume(ConsumeContext<SendMessage> context)
        {
            Interlocked.Increment(ref messageCount);
            return Task.CompletedTask;
        }
    }
}