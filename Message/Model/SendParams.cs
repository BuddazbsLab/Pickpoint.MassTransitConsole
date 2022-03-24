namespace Common
{
    public class SimpleConfig
    {
        public int NumberMessage { get; init; }
        public int SendIntervalSeconds { get; init; }
        public int NumberListener { get; init; }
        public int MessageTextSizeBytes { get; init; }

    }
    public class TrafficParams
    {
        public int SendIntervalSeconds { get; init; }
        public int InputTrafficSizeInMbPerSecond { get; init; }
        public int OutputTrafficSizeInMbPerSecond { get; init; }
        public int MessageTextSizeBytes { get; init; }
    }

    public class BasicRMQParam
    {
        public int NumberMessage { get; init; }
        public int SendIntervalSeconds { get; init; }
        public int MessageTextSizeBytes { get; init; }
    }
}
