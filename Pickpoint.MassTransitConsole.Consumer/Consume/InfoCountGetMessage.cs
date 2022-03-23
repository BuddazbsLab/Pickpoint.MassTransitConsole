using NLog;

namespace Pickpoint.MassTransitConsole.Consumer.Consume
{
    sealed internal class InfoCountGetMessage
    {
        public InfoCountGetMessage(Logger logger, int countGetMessage)
        {
            this.Logger = logger;
            this.CountGetMessage = countGetMessage;
        }

        public int CountGetMessage { get; }
        private Logger Logger { get; }

        public Task CountMessage(int countGetMessage)
        {
            Logger.Info($"[*]Messages received {countGetMessage}.");
            return Task.CompletedTask;
        }
    }
}
