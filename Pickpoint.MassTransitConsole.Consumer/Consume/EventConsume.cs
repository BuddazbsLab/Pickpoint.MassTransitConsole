using MassTransit;
using Message;
using NLog;

namespace Pickpoint.MassTransitConsole.Consumer.Consume
{

    class EventConsumer : IConsumer<SendMessage>
    {
        private object lockObj = new();
        public static int messageCount = 0;


        private static Logger logger = LogManager.GetCurrentClassLogger();
        public Task Consume(ConsumeContext<SendMessage> context)
        {
            lock (lockObj)
            {
                Interlocked.Increment(ref messageCount);
            }
            
            logger.Info("Получено сообщение: {0}.", context.Message.Text);
            logger.Info("Всего сообщений: {0}.", messageCount);

            return Task.CompletedTask;
        }
    }
}

