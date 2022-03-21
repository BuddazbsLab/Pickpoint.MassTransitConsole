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

        public async Task<UsingRabbitMqConfig> GetSettingsAppRMQ()
        {
            return ConfigureProvider.GetSection("Rabbit").Get<UsingRabbitMqConfig>();
        }
        public async Task<SendParams> SendParams()
        {
            return ConfigureProvider.GetSection("SendMessageParam").Get<SendParams>();
        }

    }
}
