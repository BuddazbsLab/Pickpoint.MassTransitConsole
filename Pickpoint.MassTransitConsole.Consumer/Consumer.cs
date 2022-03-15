using MassTransit;
using MassTransit.Testing;
using Message;
using Newtonsoft.Json;
using NLog;
using RabbitMqConfig;
using System.Diagnostics;

namespace Pickpoint.MassTransitConsole.Consumer
{
    internal class Consumer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static async Task EventListener()
        {
            var items = JsonConvert.DeserializeObject<UsingRabbitMqConfig>(await File.ReadAllTextAsync("appsettings.json"));

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host($"amqp://{items.host}", h =>
                {
                    h.Password(items.password);
                    h.Username(items.userName);
                });

                cfg.Message<IValueEntered>(x => { x.SetEntityName("Consumer"); });

                cfg.ReceiveEndpoint("event-listener", e =>
                {
                    e.Consumer<EventConsumer>();

                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(15));
            var timer = new Stopwatch();
            timer.Start();
            try
            {
                
                logger.Info("Зпущено приложение для получения сообщений, или нажмите [enter] для выхода.");
                logger.Info("Ожидаю сообщения."); 
                
                await busControl.StartAsync(source.Token);

                await Task.Run(() => Console.ReadLine());

                timer.Stop();
            }
            finally
            {
                await busControl.StopAsync(source.Token);                
            }
        }

        class EventConsumer :
            IConsumer<IValueEntered>
        {
            public Task Consume(ConsumeContext<IValueEntered> context)
            {
                logger.Info("Получено сообщение: {0}.", context.Message.Text);
                return Task.CompletedTask;
            }
        }
    }
}

