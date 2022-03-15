using MassTransit;
using MassTransit.Testing;
using Message;
using Newtonsoft.Json;
using System.Diagnostics;
using RabbitMqConfig;
using NLog;

namespace Pickpoint.MassTransitConsole.Publisher
{
    internal class Publisher
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static  async Task ConnectionAndSendMessage()
        {
            var items = JsonConvert.DeserializeObject<UsingRabbitMqConfig>(await File.ReadAllTextAsync("appsettings.json"));

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {               
                        cfg.Host($"amqp://{items.host}", h =>
                        {
                            h.Password(items.password);
                            h.Username(items.userName);  
                        });
            });
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);

            try
            {
                logger.Info("Запущено приложние для отправки сообщений (Publisher)");

                // отправить сообщение потребителю (consumers)
                
                var endpoint = await busControl.GetSendEndpoint(new Uri("exchange:Consumer"));

                // Задаем сколько сообщений нужно отправить
                var messageStart = items.numberMessage;

                var timer = new Stopwatch();
                timer.Start();
                // 1 сообщение 875 bytes (Статистика из кролика)
                for (int i = 0; i < messageStart; i++)
                {
                    await endpoint.Send<IValueEntered>(new
                    {
                        Text = "123",
                    });
                }
                logger.Info($"Отпралено {messageStart} сообщений за {timer.ElapsedMilliseconds:N0} ms");
                logger.Info($"Размер сообщений составляет: {messageStart*875} bytes");                
                timer.Stop();

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
