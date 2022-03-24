using Common;
using NLog;
using Pickpoint.RabbitMQ.Publisher.SizeText;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;


namespace Pickpoint.RabbitMQ.Publisher
{
    sealed internal class SendMessageRMQ
    {
        public SendMessageRMQ(Logger logger)
        {
            this.Logger = logger;
        }

        private Logger Logger { get; }

        public async Task InitSendMessage(ConnectionFactory initRabbitMQ, UsingRabbitMqConfig config, BasicRMQParam paramsendRMQ)
        {
            using (var connection = initRabbitMQ.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var messageText = GenerateString.generateASCIIStringBySize(paramsendRMQ.MessageTextSizeBytes);
                var messageBodyBytes = Encoding.UTF8.GetBytes(messageText);

                IBasicProperties props = channel.CreateBasicProperties();

                props.DeliveryMode = 2; // режим доставки постоянный (свойтсво 2)

                var timer = new Stopwatch();
                timer.Start();

                var limitMessage = paramsendRMQ.NumberMessage;
                var basicTimeNeedSendMessage = paramsendRMQ.SendIntervalSeconds * 10; // Получаем нужный дилей для отправки сообщений за указанный промежуток времени
                this.Logger.Info($"[*]Подождите. Идет процесс отправки сообщений.");
                if (limitMessage > 1000)
                {
                    for (int i = 0; i < limitMessage; i++)
                    {
                        channel.BasicPublish(
                        exchange: config.ExchangesName,
                        routingKey: "",
                        props,
                        messageBodyBytes);

                        await Task.Delay(basicTimeNeedSendMessage);
                    }
                }
                else
                {
                    for (int i = 0; i < limitMessage; i++)
                    {
                        channel.BasicPublish(
                        exchange: "Consumer",
                        routingKey: "",
                        props,
                        messageBodyBytes);

                        await Task.Delay(10);
                    }
                }
                timer.Stop();

                var messageInKb = paramsendRMQ.NumberMessage * paramsendRMQ.MessageTextSizeBytes / 1024;
                var messageInMB = messageInKb / 1024;

                this.Logger.Info($"[*]Отпралено {paramsendRMQ.NumberMessage} сообщений.");
                this.Logger.Info($"[*]Затрачено времени на отправку сообщений {timer.ElapsedMilliseconds/1000} секунд");
                this.Logger.Info($"[*]Общий размер сообщений составляет {messageInKb} Килобайт или {messageInMB} Мегабайт");

            }
        }
    }
}
