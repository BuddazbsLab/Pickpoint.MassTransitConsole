using Common;
using Pickpoint.MassTransitConsole.Publisher.SizeTextSend;

namespace Pickpoint.MassTransitConsole.Publisher
{
    public class ByAmountConfigurationProvider : ISendConfigurationProvider
    {

        public ByAmountConfigurationProvider(Settings configSettings)
        {
            this.ConfigSettings = configSettings;
        }

        public Settings ConfigSettings { get; }

        public InnerSendConfig GetConfig()
        {
            var config = this.ConfigSettings.ConfigurationByAmount();
           // var intervalMilliseconds = (int)((double)config.MessageNumber / config.SendIntervalSeconds;

            var messageText = GenerateString.generateASCIIStringBySize(config.MessageTextSizeBytes);
            return new InnerSendConfig
            {
                Message = new SendMessage { Text = messageText },
                MessageNumber = config.NumberMessage,
                TimeIntervalMilliseconds = config.SendIntervalSeconds/* * 1000*/
            };
        }
    }
}
