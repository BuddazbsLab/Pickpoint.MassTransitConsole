using Common;
using MassTransit;

namespace Pickpoint.MassTransitConsole.Consumer.Consume
{

    class EventConsumer : IConsumer<ISendMessage>
    {
        public static int MessageCount
        {
            get
            {
                return messageCount;
            }
        }

        private static int messageCount;

        public Task Consume(ConsumeContext<ISendMessage> context)
        {
            Interlocked.Increment(ref messageCount);
            return Task.CompletedTask;
        }
    }
}