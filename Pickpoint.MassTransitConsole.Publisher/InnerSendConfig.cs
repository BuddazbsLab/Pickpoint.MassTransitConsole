using Common;

namespace Pickpoint.MassTransitConsole.Publisher
{
    public class InnerSendConfig
    {
        public int TimeIntervalMilliseconds { get; set; }
        public int MessageNumber { get; set; }
        public SendMessage Message { get; set; }
    }
}
