
using Common;
using NLog;
using RabbitMQ.Client;

namespace Pickpoint.RabbitMQ.Publisher
{
    sealed internal class RabbitMQConfiguration
    {
        public RabbitMQConfiguration(Logger logger)
        {
            this.Logger = logger;
        }

        private Logger Logger { get; }

        public async Task<ConnectionFactory> InitializeConfigure(UsingRabbitMqConfig config)
        {
            this.Logger.Info("[*]Starting RabbitMQ app.......");
            var rmqControl = new ConnectionFactory()
            {
                HostName = config.host.ToString(),
                UserName = config.userName,
                Password = config.password,
                Port = Convert.ToInt32(config.port)
            };

            this.Logger.Info("[*]RabbitMQ: configured and started!");
            return rmqControl;
        }

    }
}
