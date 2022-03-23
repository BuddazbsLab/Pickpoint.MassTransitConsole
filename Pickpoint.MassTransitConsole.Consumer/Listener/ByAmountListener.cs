using Common;
using NLog;
using Pickpoint.MassTransitConsole.Consumer.Consume;

namespace Pickpoint.MassTransitConsole.Consumer.Listener
{
  sealed  internal class ByAmountListener
    {
        private readonly int _additionalDelayTimeByAmount = 1050;

        public ByAmountListener(Settings settings, Logger logger)
        {
            this.Settings = settings;
            this.Logger = logger;
        }

        private Settings Settings { get; }
        private Logger Logger { get; }

        public async Task ByAmount()
        {
            var delayReciveMessageByAmount = (this.Settings.ConfigurationByAmount().SendIntervalSeconds * _additionalDelayTimeByAmount);

            await Task.Delay(delayReciveMessageByAmount);
            var countGetMessageByAmount = EventConsumer.MessageCount;
            InfoCountGetMessage infoCountGetMessageByAmount = new(this.Logger, countGetMessageByAmount);
            await infoCountGetMessageByAmount.CountMessage(countGetMessageByAmount);
        }
    }
}
