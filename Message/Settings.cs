using Common.Model;
using Microsoft.Extensions.Configuration;

namespace Common
{
    public class Settings
    {
        public Settings(IConfiguration configureProvider)
        {
            this.ConfigureProvider = configureProvider;
        }

        public IConfiguration ConfigureProvider { get; }

        public UsingRabbitMqConfig GetSettingsAppRMQ()
        {
            return ConfigureProvider.GetSection("Rabbit").Get<UsingRabbitMqConfig>();
        }
        public SimpleConfig ConfigurationByAmount()
        {
            return ConfigureProvider.GetSection("ConfigurationByAmount").Get<SimpleConfig>();
        }

        public TrafficParams ConfigurationByTraffic()
        {
            return ConfigureProvider.GetSection("ConfigurationByTraffic").Get<TrafficParams>();
        }

        public ConfigurationTypes ConfigurationType()
        {
            return ConfigureProvider.GetSection("ConfigurationType").Get<ConfigurationTypes>();
        }

        public BasicRMQParam ConfigurationBasicSendRMQ()
        {
            return ConfigureProvider.GetSection("SendParamMessageRMQ").Get<BasicRMQParam>();
        }
    }
}
