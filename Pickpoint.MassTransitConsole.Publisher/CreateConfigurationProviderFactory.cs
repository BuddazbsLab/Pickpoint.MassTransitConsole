using Common;
using Common.Model;

namespace Pickpoint.MassTransitConsole.Publisher
{
    sealed public class CreateConfigurationProviderFactory 
    {
        public CreateConfigurationProviderFactory(Settings settings)
        {
            Settings = settings;
        }

        private Settings Settings { get; }

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
