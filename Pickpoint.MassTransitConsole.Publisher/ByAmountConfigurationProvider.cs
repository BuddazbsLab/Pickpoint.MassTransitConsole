using Common;
using Pickpoint.MassTransitConsole.Publisher.SizeTextSend;

namespace Pickpoint.MassTransitConsole.Publisher
{
    sealed public class ByAmountConfigurationProvider : ISendConfigurationProvider
    {

        public ByAmountConfigurationProvider(Settings configSettings)
        {
            this.ConfigSettings = configSettings;
        }

        private Settings ConfigSettings { get; }

        public InnerSendConfig GetConfig()
        {
            var config = this.ConfigSettings.ConfigurationByAmount();
           

            var messageText = GenerateString.generateASCIIStringBySize(config.MessageTextSizeBytes);
            return new InnerSendConfig
            {
                Message = new SendMessage { Text = messageText },
                MessageNumber = config.NumberMessage,
                TimeIntervalMilliseconds = config.SendIntervalSeconds
            };
        }
    }
}
