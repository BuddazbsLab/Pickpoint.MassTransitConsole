using MassTransit;
using NLog;
using System.Diagnostics;

namespace Pickpoint.MassTransitConsole.Publisher
{
    sealed internal partial class MessageSender
    {
        private readonly int _translationInSeconds = 1000;

        public MessageSender(Logger logger, IBusControl massTransitBusControl, ISendConfigurationProvider sendConfigurationProvider)
        {
            this.Logger = logger;
            this.MassTransitBusControl = massTransitBusControl;
            this.SendConfig = sendConfigurationProvider.GetConfig();
        }

        private Logger Logger { get; }
        private IBusControl MassTransitBusControl { get; }
        private InnerSendConfig SendConfig { get; }

        public const int baseMessageSizeBytes = 875;
        public async Task Send()
        {
            this.Logger.Info("[*]The application for sending messages is running (Publisher)");
            this.Logger.Info("[*]The process of sending messages is underway. Please wait");
            var endpoint = await this.MassTransitBusControl.GetSendEndpoint(new Uri("exchange:Consumer"));
           
            var timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < SendConfig.MessageNumber; i++)
            {   
             await endpoint.Send(SendConfig.Message);
             await Task.Delay(SendConfig.TimeIntervalMilliseconds);
            }
            timer.Stop();
            this.Logger.Info($"[*]Отпралено {SendConfig.MessageNumber} сообщений за {(int)(double)timer.ElapsedMilliseconds / _translationInSeconds} секунд(ы). Ожидаемое время ~ {SendConfig.TimeIntervalMilliseconds} секунд.");
            this.Logger.Info($"[*]Размер сообщений составляет: {SendConfig.MessageNumber * (baseMessageSizeBytes + SendConfig.Message.Text.Length) } bytes");
        }
    }
}
