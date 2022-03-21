namespace Common
{
    public class SendParams
    {
        public int MessageNumber { get; init; }
        public int SendIntervalSeconds { get; init; }
        public int numberMessage { get; set; }
        public int messageSendTimeIntervalSeconds { get; set; }
        public long trafficSendTextInBytes { get; set; }
        public string alphabet { get; set; }
        public int sendingLogic { get; set; }
        public int packageSize { get; set; }
        public int sizeTrafficInMb { get; set; }

    }
}
