using Common;
using Common.Model;
using NLog;
using Pickpoint.MassTransitConsole.Consumer.Consume;

namespace Pickpoint.MassTransitConsole.Consumer
{
    internal class CreateConfigurationListenerProvaider
    {
        public CreateConfigurationListenerProvaider(Settings settings, Logger logger)
        {
          this.Settings = settings;
          this.Logger = logger;
        }

        public Settings Settings { get; }
        public Logger Logger { get; }

        public async Task ConfigurationListenerProviderAsync(ConfigurationTypes configType)
        {
            var delayReciveMessageByAmount = (this.Settings.ConfigurationByAmount().SendIntervalSeconds * 1050);
            
            var delayReciveMessageByTraffic = (this.Settings.ConfigurationByTraffic().SendIntervalSeconds * 36000);
            

            switch (configType)
            {
                case ConfigurationTypes.ByAmount:
                    
                    await Task.Delay(delayReciveMessageByAmount);
                    var countGetMessageByAmount = EventConsumer.MessageCount;
                    InfoCountGetMessage infoCountGetMessageByAmount = new(this.Logger, countGetMessageByAmount);
                    await infoCountGetMessageByAmount.CountMessage(countGetMessageByAmount);
                    break;

                case ConfigurationTypes.ByTraffic:

                    await Task.Delay(delayReciveMessageByTraffic);
                    var countGetMessageByTraffic = EventConsumer.MessageCount;
                    InfoCountGetMessage infoCountGetMessageByTraffic = new(this.Logger, countGetMessageByTraffic);
                    await infoCountGetMessageByTraffic.CountMessage(countGetMessageByTraffic);
                    break;


                default:
                    throw new Exception("Нет подходящего слушателя!");
            }

        }

    }
}
