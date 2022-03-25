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
                this.Logger.Info($"[*]Подождите. Идет процесс отправки сообщений.");

                var timer = new Stopwatch();
                timer.Start();

                CountMessage countMessage = new CountMessage();
                await countMessage.CountAsync(paramsendRMQ, config, channel);

                timer.Stop();

                var messageInKb = paramsendRMQ.NumberMessage * paramsendRMQ.MessageTextSizeBytes / 1024;
                var messageInMB = messageInKb / 1024;

                this.Logger.Info($"[*]Отпралено {paramsendRMQ.NumberMessage} сообщений.");
                this.Logger.Info($"[*]Затрачено времени на отправку сообщений {timer.ElapsedMilliseconds / 1000} секунд");
                this.Logger.Info($"[*]Общий размер сообщений составляет {messageInKb} Килобайт или {messageInMB} Мегабайт");

            }
        }
    }
}
