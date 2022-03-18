using Common;
using Microsoft.Extensions.Configuration;

namespace Pickpoint.MassTransitConsole.Consumer
{
    internal class Settings
    {
        public Settings(IConfiguration configureProvider)
        {
            this.ConfigureProvider = configureProvider;
        }

        public IConfiguration ConfigureProvider { get; }

        public async Task<UsingRabbitMqConfig> GetSettingsApp()
        {
            return ConfigureProvider.GetSection("Rabbit").Get<UsingRabbitMqConfig>();
        }

    }
}
