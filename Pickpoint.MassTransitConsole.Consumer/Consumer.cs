using MassTransit;
using MassTransit.Testing;
using Message;
using NLog;
using Pickpoint.MassTransitConsole.Consumer.Consume;

namespace Pickpoint.MassTransitConsole.Consumer
{
    internal class Consumer
    {

    private static Logger logger = LogManager.GetCurrentClassLogger();
        public static async Task MasstransitConfigure(
                                                        Uri host,
                                                        string password,
                                                        string port,
                                                        string queueName,
                                                        string userName,
                                                        int numberListener
                                                        ) 

        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Message<SendMessage>(x => { x.SetEntityName("Consumer"); });

                /*Цикл для создания очередей и приема из них сообщений.
                 *Инициализирована переменная для укзания кол-ва очередей
                 */
                    var listener = numberListener;

                    cfg.Host($"amqp://{host}:{port}", h =>
                    {
                       h.Password(password);
                       h.Username(userName);
                    });

                for (var i = 0; i < listener; i++)
                {
                    cfg.ReceiveEndpoint($"{queueName}{i}", e =>
                    {
                        e.Consumer<EventConsumer>(c =>
                        {
                            c.UseConcurrentMessageLimit(1);
                            });
                        
                    });
                }
               
            });
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(100000));
            
            try
            {                
                logger.Info("Зпущено приложение для получения сообщений, или нажмите [enter] для выхода.");
                logger.Info("Ожидаю сообщения."); 
                
                await busControl.StartAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException("Сообщения не были получены. \nПроверь: \n1. Подключение. \n2. Привязку к очереди. \n3. Стороноу, кто шлет сообщения.");
            }
            //finally
            //{
            //    await busControl.StopAsync(); //Проблема               
            //}
        }        
    }
}

