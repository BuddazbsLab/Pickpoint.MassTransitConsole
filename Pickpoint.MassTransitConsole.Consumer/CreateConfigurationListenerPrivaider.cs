using Common;
using Common.Model;
using NLog;
using Pickpoint.MassTransitConsole.Consumer.Listener;

namespace Pickpoint.MassTransitConsole.Consumer
{
    sealed internal class CreateConfigurationListenerProvaider
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
            switch (configType)
            {
                case ConfigurationTypes.ByAmount:
                    ByAmountListener byAmountListener = new(Settings, Logger);
                    await byAmountListener.ByAmount();
                    return;

                case ConfigurationTypes.ByTraffic:
                    ByTrafficListener byTrafficListener = new(Settings, Logger);
                    await byTrafficListener.ByTraffic();
                    return;

                default:
                    throw new Exception("Нет подходящего слушателя!");
            }

        }

    }
}
