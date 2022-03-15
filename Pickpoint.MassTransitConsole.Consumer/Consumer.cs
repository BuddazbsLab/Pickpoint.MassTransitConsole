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
                cfg.Message<SendMessage>(x => { x.SetEntityName("Consumer"); });

                // Очередь по умоляанию.
                cfg.Host($"amqp://{items.host}:{items.port}", h =>
                {
                    h.Password(items.password);
                    h.Username(items.userName);
                });
                cfg.ReceiveEndpoint($"{items.queueName}", e =>
                {
                    e.Consumer<EventConsumer>();
                });

                /*Цикл для создания очередей и приема из них сообщений.
                 *Инициализирована переменная для укзания кол-ва очередей
                 */
                var listener = items.numberListener;

                for (var i = 0; i < listener; i++)
                {
                    cfg.Host($"amqp://{items.host}:{items.port}", h =>
                {
                    h.Password(items.password);
                    h.Username(items.userName);
                });
                    cfg.ReceiveEndpoint($"{items.queueName}{i}", e =>
                    {
                        e.Consumer<EventConsumer>();
                    });
                }
            });

            var timer = new Stopwatch();
            timer.Start();
            try
            {
                
                logger.Info("Зпущено приложение для получения сообщений, или нажмите [enter] для выхода.");
                logger.Info("Ожидаю сообщения."); 
                
                await busControl.StartAsync();

                timer.Stop();
            }
            catch (Exception)
            {
                throw new ArgumentException("Сообщения не были получены. \nПроверь: \n1. Подключение. \n2. Привязку к очереди. \n3. Стороноу, кто шлет сообщения.");
            }
            finally
            {
                await busControl.StopAsync();                
            }
        }

        class EventConsumer :
            IConsumer<SendMessage>
        {
            public Task Consume(ConsumeContext<SendMessage> context)
            {
                logger.Info("Получено сообщение: {0}.", context.Message.Text);
                return Task.CompletedTask;
            }
        }
    }
}

