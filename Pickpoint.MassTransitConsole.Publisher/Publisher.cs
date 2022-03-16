using MassTransit;
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
        
        public static  async Task ConnectionAndSendMessage(
                                                        Uri host,
                                                        string password,
                                                        string port,
                                                        string userName,
                                                        int numberMessage)
        {

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {

                cfg.Message<SendMessage>(x => { x.SetEntityName("Publisher"); });

                cfg.Host($"amqp://{host}:{port}", h =>
                        {
                            h.Password(password);
                            h.Username(userName);  
                        });
            });

            await busControl.StartAsync();

            try
            {
                logger.Info("Запущено приложние для отправки сообщений (Publisher)");

                // отправить сообщение потребителю (consumers)
                 var endpoint = await busControl.GetSendEndpoint(new Uri("exchange:Consumer"));
                    

                // Задаем сколько сообщений нужно отправить
                var messageStart = numberMessage;

                var timer = new Stopwatch();
                timer.Start();
                // 1 сообщение 875 bytes (Статистика из кролика)
                for (int i = 0; i < messageStart; i++)
                {
                    await endpoint.Send<SendMessage>(new
                    {
                        Text = "123"
                    });

                    //Примерно 2 мс нужно для отправки сообщения. 
                    await Task.Delay(messageStart/2);
                }
                logger.Info($"Отпралено {messageStart} сообщений за {timer.ElapsedMilliseconds:N0} ms");
                logger.Info($"Размер сообщений составляет: {messageStart*875} bytes");                
                timer.Stop();                
            }
            finally
            {
                await busControl.StopAsync();
            }

        }
    }
}
