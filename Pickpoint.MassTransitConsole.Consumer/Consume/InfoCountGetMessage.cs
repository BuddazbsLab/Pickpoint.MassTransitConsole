using NLog;

namespace Pickpoint.MassTransitConsole.Consumer.Consume
{
    internal class InfoCountGetMessage
    {
        public InfoCountGetMessage(Logger logger, int countGetMessage)
        {
            this.Logger = logger;
            this.CountGetMessage = countGetMessage;
        }

        public int CountGetMessage { get; }
        internal Logger Logger { get; }

        public async Task CountMessage(int countGetMessage)
        {
            this.Logger.Info($"[*]Messages received {countGetMessage}.");
        }
    }
}
