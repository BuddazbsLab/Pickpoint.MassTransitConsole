using Common;
using MassTransit;
using NLog;

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


        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Task Consume(ConsumeContext<ISendMessage> context)
        {

            Interlocked.Increment(ref messageCount);
            return Task.CompletedTask;
        }
    }
}