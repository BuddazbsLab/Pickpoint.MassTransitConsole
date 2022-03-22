using Common;
using MassTransit;
using NLog;
using System.Diagnostics;

namespace Pickpoint.MassTransitConsole.Publisher
{
    internal class MessageSender
    {
        public MessageSender(Logger logger, IBusControl massTransitBusControl, string sendTrafficText , int sendingLogic, int packageSize, int sizeTrafficInMb)
        {
            this.Logger = logger;
            this.MassTransitBusControl = massTransitBusControl;
            this.SendTrafficText = sendTrafficText;
            this.SendingLogic = sendingLogic;
            this.PackageSize = packageSize;
            this.SizeTrafficInMb = sizeTrafficInMb;
        }

        internal Logger Logger { get; }
        internal IBusControl MassTransitBusControl { get; }
        internal string SendTrafficText { get; }
        public int SendingLogic { get; }
        public int PackageSize { get; }
        public int SizeTrafficInMb { get; }

        public async Task Send(SendParams sendConfig)
        {
            try
            {
                this.Logger.Info("[*]The application for sending messages is running (Publisher)");
                this.Logger.Info("[*]The process of sending messages is underway. Please wait");
                // отправить сообщение потребителю (consumers)
                var endpoint = await this.MassTransitBusControl.GetSendEndpoint(new Uri("exchange:Consumer"));

                var timer = new Stopwatch();
                timer.Start();

                var messageNumber = sendConfig.MessageNumber;
                var intervalMilliseconds = (int)((double)messageNumber / sendConfig.SendIntervalSeconds * 1000);


                switch (SendingLogic)
                {
                    case 1:
                        // 1 сообщение 875 bytes (Статистика из кролика)
                        for (int i = 0; i < messageNumber; i++)
                        {
                            await endpoint.Send<SendMessage>(new
                            {
                                Text = SendTrafficText
                            });
                        }

                        await Task.Delay(intervalMilliseconds);
                        timer.Stop();
                        this.Logger.Info($"[*]Отпралено {messageNumber} сообщений за {(double)timer.ElapsedMilliseconds / 1000} секунд. Ожидаемое время ~ {sendConfig.SendIntervalSeconds} секунд.");
                        this.Logger.Info($"[*]Размер сообщений составляет: {messageNumber * 875} bytes");

                        break;

                    case 2:
                        var sizeByteInMb = 1048576;
                        var sendmessageNumber = SizeTrafficInMb * sizeByteInMb / PackageSize ; // перевод в МЬ
                        for (int i = 0; i < sendmessageNumber; i++)
                        {
                            await endpoint.Send<SendMessage>(new
                            {
                                Text = SendTrafficText
                            });
                        }
                        await Task.Delay(intervalMilliseconds);
                        timer.Stop();
                        this.Logger.Info($"[*]Отпралено {sendmessageNumber} сообщений за {(double)timer.ElapsedMilliseconds / 1000} секунд.");
                        this.Logger.Info($"[*]Размер сообщений составляет: {sendmessageNumber * 875} bytes");

                        break;
                }               
            }
            finally
            {
                await this.MassTransitBusControl.StopAsync();
            }
        }
    }
}
