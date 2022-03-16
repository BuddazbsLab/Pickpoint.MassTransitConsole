

using Newtonsoft.Json;
using RabbitMqConfig;

namespace Pickpoint.MassTransitConsole.Publisher
{
    internal class Settings
    {
        public async Task GetSettingsApp()
        {
            var items = JsonConvert.DeserializeObject<UsingRabbitMqConfig>(await File.ReadAllTextAsync("appsettings.json"));

            await Publisher.ConnectionAndSendMessage(
                items.host,
                items.password,
                items.port,
                items.userName,
                items.numberMessage);
        }
    }
}
