using MassTransit;
using MassTransit.Testing;
using Message;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using RabbitMqConfig;

namespace Pickpoint.MassTransitConsole.Publisher
{
    internal class Publisher
    {
        public static  async Task ConnectionAndSendMessage()
        {
            var items = JsonConvert.DeserializeObject<UsingRabbitMqConfig>(await File.ReadAllTextAsync("appsettings.json"));

            var busControl = Bus.Factory.CreateUsingRabbitMq(async cfg =>
            {               
                        cfg.Host($"amqp://{items.host}", h =>
                        {
                            h.Password(items.password);
                            h.Username(items.userName);  
                        });
            });
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            var monitor = busControl.CreateBusActivityMonitor();

            try
            {
                Console.WriteLine("Запущено приложние для отправки сообщений (Publisher)");

                // отправить сообщение потребителю (consumers)
                
                var endpoint = await busControl.GetSendEndpoint(new Uri("exchange:Consumer"));

                var messageStart = items.numberMessage;
                var timer = new Stopwatch();
                timer.Start();

                // 1 сообщение 875 bytes (Статистика из кролика)
                for(int i = 0; i < messageStart; i++)
                {
                    await endpoint.Send<IValueEntered>(new
                    {
                        Text = "123",
                    });
                }             
                Console.WriteLine($"Отпралено {messageStart} сообщений за {timer.ElapsedMilliseconds:N0} ms");
                timer.Stop();
                await monitor.AwaitBusInactivity();
                // точка остановки. не блокирует основной поток программы
                await Task.Run(Console.ReadLine);
            }
            finally
            {
                await busControl.StopAsync(CancellationToken.None);
            }

        }
    }
}
