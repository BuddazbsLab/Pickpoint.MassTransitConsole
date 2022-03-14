using MassTransit;
using Message;
using Newtonsoft.Json;
using NLog;
using RabbitMqConfig;

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

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                logger.Info("Зпущено приложение для получения сообщений, или нажмите [enter] для выхода.");
                logger.Info("Ожидаю сообщения.");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }

        class EventConsumer :
            IConsumer<IValueEntered>
        {
            public async Task Consume(ConsumeContext<IValueEntered> context)
            {

                logger.Info("Получено сообщение: {0}.", context.Message.Text);
                
            }

        }
    }
}

