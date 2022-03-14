using MassTransit;
using Message;

namespace Pickpoint.MassTransitConsole.Consumer
{
    internal class Consumer
    {
        public static async Task EventListener()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", h =>
                {
                    h.Password("guest");
                    h.Username("guest");
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
                Console.WriteLine("[*] Зпущено приложение для получения сообщений, или нажмите [enter] для выхода.");
                Console.WriteLine("[*] Ожидаю сообщения.");

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
                
                    Console.WriteLine("Получено сообщение: {0}", context.Message.Text);
                
            }

        }
    }
}

