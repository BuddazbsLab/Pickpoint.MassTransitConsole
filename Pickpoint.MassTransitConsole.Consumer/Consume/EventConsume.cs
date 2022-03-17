using MassTransit;
using Message;
using NLog;

namespace Pickpoint.MassTransitConsole.Consumer.Consume
{

    class EventConsumer : IConsumer<ISendMessage>
    {
        private object lockObj = new();
        public static int messageCount { get; private set; }


        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Task Consume(ConsumeContext<ISendMessage> context)
        {

            logger.Info("Получено сообщение: {0}.", context.Message.Text);

            lock (lockObj)
            {
                messageCount++;
            }

            var numberMessage = MessageCount.messageNumber;

            if (messageCount == numberMessage)
            {
                logger.Info($"Всего получено сообщений {messageCount} из возможных {numberMessage}.");
            }
            return Task.CompletedTask;
        }
    }
}