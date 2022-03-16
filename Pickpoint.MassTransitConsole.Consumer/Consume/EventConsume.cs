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
            logger.Info("Получено сообщение: {0}.", context.Message.Text);          
            
            lock (lockObj)
            {
                Interlocked.Increment(ref messageCount);                
            }
            if(messageCount == 100)
            {
                logger.Info("Всего сообщений получено: {0}.", messageCount);
            }
            else
            {
                logger.Info("Получено {0} сообщений из {1} возможных: .", messageCount);
            }

            return Task.CompletedTask;

        }
    }
}

