using Common;
using NLog;
using Pickpoint.MassTransitConsole.Consumer.Consume;

namespace Pickpoint.MassTransitConsole.Consumer.Listener
{
  sealed  internal class ByTrafficListener
    {
        private readonly int _additionalDelayTimeByTraffic = 36000;

        public ByTrafficListener(Settings settings, Logger logger)
        {
            this.Settings = settings;
            this.Logger = logger;
        }

        public Settings Settings { get; }
        public Logger Logger { get; }


        public async Task ByTraffic()
        {
            var delayReciveMessageByTraffic = (this.Settings.ConfigurationByTraffic().SendIntervalSeconds * _additionalDelayTimeByTraffic);

            await Task.Delay(delayReciveMessageByTraffic);
            var countGetMessageByAmount = EventConsumer.MessageCount;
            InfoCountGetMessage infoCountGetMessageByTraffic = new(this.Logger, countGetMessageByAmount);
            await infoCountGetMessageByTraffic.CountMessage(countGetMessageByAmount);
        }
    }
}
