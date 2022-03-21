namespace Common
{
    public class UsingRabbitMqConfig
    {
        public string userName { get; set; }
        public string password { get; set; }
        public Uri host { get; set; }
        public string port { get; set; }
        public int numberMessage { get; set; }
        public string queueName { get; set; }
        public int numberListener { get; set; }
        public int messageSendTimeIntervalSeconds { get; set; }
        public long trafficSendinBytes { get; set; }
        public string alphabet  { get; set; }
    }
}


