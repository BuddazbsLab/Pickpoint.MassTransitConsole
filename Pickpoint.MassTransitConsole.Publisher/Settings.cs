using Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Pickpoint.MassTransitConsole.Publisher
{
    internal class Settings
    {
        public Settings(IConfiguration configProvider)
        {
            this.ConfigProvider = configProvider;
        }

        public IConfiguration ConfigProvider { get; }

        public async Task<UsingRabbitMqConfig> GetSettingsApp()
        {
            return ConfigProvider.GetSection("Rabbit").Get<UsingRabbitMqConfig>();
        }
    }
}
