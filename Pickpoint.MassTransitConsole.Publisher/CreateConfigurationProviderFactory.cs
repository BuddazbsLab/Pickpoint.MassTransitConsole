using Common;
using Common.Model;

namespace Pickpoint.MassTransitConsole.Publisher
{
    public class CreateConfigurationProviderFactory 
    {
        public CreateConfigurationProviderFactory(Settings settings)
        {
            Settings = settings;
        }

        public Settings Settings { get; }

        public  ISendConfigurationProvider ConfigurationProvider(ConfigurationTypes configType)
        {

            switch (configType)
            {
                case ConfigurationTypes.ByAmount:
                    {
                        return new ByAmountConfigurationProvider(this.Settings);
                    }
                case ConfigurationTypes.ByTraffic:
                    return new ByTrafficConfigurationProvider(this.Settings);
                default:
                    throw new Exception("Нет подходящего провайдера!");
            }
        }
    }
}
