using Newtonsoft.Json;
using RabbitMqConfig;

namespace Pickpoint.MassTransitConsole.Consumer
{
    internal class Settings
    {
        public async Task GetSettingsApp()
        {
            var items = JsonConvert.DeserializeObject<UsingRabbitMqConfig>(await File.ReadAllTextAsync("appsettings.json"));

            await Consumer.MasstransitConfigure(
                items.host,
                items.password,
                items.port,
                items.queueName,
                items.userName,
                items.numberListener);
        }

    }
}
