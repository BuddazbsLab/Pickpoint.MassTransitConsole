using Common;
using Pickpoint.MassTransitConsole.Publisher.SizeTextSend;

namespace Pickpoint.MassTransitConsole.Publisher
{
    sealed public class ByTrafficConfigurationProvider : ISendConfigurationProvider
    {
        private const int _sizeOfMbInBytes = 1048576;
        private const int _basePackageSize = 871;
        private readonly int _translationInMilliseconds = 1000;

        public ByTrafficConfigurationProvider(Settings configSettings)
        {
            this.ConfigSettings = configSettings;
        }

        public ByTrafficConfigurationProvider(TrafficParams trafficParams)
        {
            TrafficParams = trafficParams;
        }

        private Settings ConfigSettings { get; }
        public TrafficParams TrafficParams { get; }

        public InnerSendConfig GetConfig()
        {
            var config = this.ConfigSettings.ConfigurationByTraffic();

            var messageText = GenerateString.generateASCIIStringBySize(config.MessageTextSizeBytes);
            var sendmessageNumber = (config.InputTrafficSizeInMbPerSecond * _sizeOfMbInBytes) / (_basePackageSize) ; // отправляет указанное кол-во Mb сообщениями
            var intervalMilliseconds = (int)(config.SendIntervalSeconds * _translationInMilliseconds / (sendmessageNumber));

            return new InnerSendConfig
            {
                Message = new SendMessage { Text = messageText },
                MessageNumber = sendmessageNumber,
                TimeIntervalMilliseconds = intervalMilliseconds + 30
            };
        }
    }
}
