

using Common;
using Pickpoint.RabbitMQ.Publisher.SizeText;
using RabbitMQ.Client;
using System.Text;

namespace Pickpoint.RabbitMQ.Publisher
{
  sealed  internal class CountMessage
    {
        public async Task CountAsync(BasicRMQParam paramsendRMQ, UsingRabbitMqConfig config, IModel channel, IConnection connection)
        {

            var limitMessage = paramsendRMQ.NumberMessage;
            var basicTimeNeedSendMessage = paramsendRMQ.SendIntervalSeconds * 10; // Получаем нужный дилей для отправки сообщений за указанный промежуток времени
            var messageText = GenerateString.generateASCIIStringBySize(paramsendRMQ.MessageTextSizeBytes);
            var messageBodyBytes = Encoding.UTF8.GetBytes(messageText);
            var firstDelayparam = limitMessage * 30;
            var secondDelayparam = limitMessage * paramsendRMQ.SendIntervalSeconds;
            var caustomTimeSendMessage = (firstDelayparam / secondDelayparam)*10;

            IBasicProperties props = channel.CreateBasicProperties();

            props.DeliveryMode = 2; // режим доставки постоянный (свойтсво 2)


            if (limitMessage > 1000)
            {
                for (int i = 0; i < limitMessage; i++)
                {
                    channel.BasicPublish(
                    exchange: config.ExchangesName,
                    routingKey: "",
                    props,
                    messageBodyBytes);

                    //await Task.Delay(TimeSpan.FromMilliseconds(1));
                }
            }
            else
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
        }
    }
}
