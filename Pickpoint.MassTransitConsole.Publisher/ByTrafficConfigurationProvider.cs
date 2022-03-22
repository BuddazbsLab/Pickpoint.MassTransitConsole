using Common;
using Pickpoint.MassTransitConsole.Publisher.SizeTextSend;

namespace Pickpoint.MassTransitConsole.Publisher
{
    public class ByTrafficConfigurationProvider : ISendConfigurationProvider
    {
        public ByTrafficConfigurationProvider(Settings configSettings)
        {
            this.ConfigSettings = configSettings;
        }

        public ByTrafficConfigurationProvider(TrafficParams trafficParams)
        {
            TrafficParams = trafficParams;
        }

        public Settings ConfigSettings { get; }
        public TrafficParams TrafficParams { get; }

        public const int SizeOfMbInBytes = 1048576;
        public const int BasePackageSize = 871;

        public InnerSendConfig GetConfig()
        {
            var config = this.ConfigSettings.ConfigurationByTraffic();

            var messageText = GenerateString.generateASCIIStringBySize(config.MessageTextSizeBytes);
            var sendmessageNumber = (config.InputTrafficSizeInMbPerSecond * SizeOfMbInBytes) / (BasePackageSize) ; // отправляет указанное кол-во Mb сообщениями
            var intervalMilliseconds = (int)(config.SendIntervalSeconds * 1000 / (sendmessageNumber));

            return new InnerSendConfig
            {
                Message = new SendMessage { Text = messageText },
                MessageNumber = sendmessageNumber,
                TimeIntervalMilliseconds = intervalMilliseconds + 30
            };
        }
    }
}
