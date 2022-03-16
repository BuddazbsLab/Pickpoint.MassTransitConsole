using MassTransit;
using Message;
using NLog;

namespace Pickpoint.MassTransitConsole.Consumer.Consume
{

    class EventConsumer : IConsumer<ISendMessage>
    {
        private object lockObj = new();
        private static int messageCount = 0;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Task Consume(ConsumeContext<ISendMessage> context)
        {

            logger.Info("Получено сообщение: {0}.", context.Message.Text);          
            
            lock (lockObj)
            {
                Interlocked.Increment(ref messageCount);                
            }
            if(messageCount == 100)
            {
                logger.Info($"Всего получено сообщений {messageCount} из возможных {100}.");
            }
            return Task.CompletedTask;

        }
    }
}

