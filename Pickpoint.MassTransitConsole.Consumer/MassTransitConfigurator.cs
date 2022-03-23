using Common;
using MassTransit;
using MassTransit.Testing;
using NLog;
using Pickpoint.MassTransitConsole.Consumer.Consume;

namespace Pickpoint.MassTransitConsole.Consumer
{
    sealed internal class MassTransitConfigurator
    {
        public MassTransitConfigurator(Logger logger, Settings settings)
        {
            this.Logger = logger;
            this.Settings = settings;
        }
        private Logger Logger { get; }
        private Settings Settings { get; }

        public async Task<IBusControl> MasstransitConfigure(UsingRabbitMqConfig config) 

        {
            this.Logger.Info("[*]Starting MassTransit app.......");
            await Task.Delay(10);

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Message<SendMessage>(x => { x.SetEntityName("Consumer"); });

                #region Переменные с данными подключения
                var listener = this.Settings.ConfigurationByAmount().NumberListener;
                var host = config.host;
                var port = config.port;
                var password = config.password;
                var userName = config.userName;
                var queueName = config.queueName;
                #endregion

                cfg.Host($"amqp://{host}:{port}", h =>
                    {
                       h.Password(password);
                       h.Username(userName);
                    });

                for (var i = 0; i < listener; i++)
                {
                    cfg.ReceiveEndpoint($"{queueName}{i}", e =>
                    {
                        e.Consumer<EventConsumer>(c =>
                        {
                            c.UseConcurrentMessageLimit(1);                            
                        });                       
                    });
                }              
            });

            try
            {
                await busControl.StartAsync();

                #region Информативыные сообщения о работе приложения
                this.Logger.Info("[*]MassTransit configured started!");
                this.Logger.Info("[*]The application for received messages is running (Consumer)");
                this.Logger.Info("[*]Await Message......");
                #endregion

                return busControl;
            }
            catch (Exception)
            {
                throw new ArgumentException("Сообщения не были получены. \nПроверь: \n1. Подключение. \n2. Привязку к очереди. \n3. Стороноу, кто шлет сообщения.");
            }
        }
    }
}

