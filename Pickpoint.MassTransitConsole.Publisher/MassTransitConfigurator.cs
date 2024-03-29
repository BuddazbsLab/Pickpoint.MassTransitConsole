﻿using Common;
using MassTransit;
using NLog;

namespace Pickpoint.MassTransitConsole.Publisher
{
    sealed internal class MassTransitConfigurator
    {
        public MassTransitConfigurator(Logger logger)
        {
            this.Logger = logger;
        }

        private Logger Logger { get; }

        public async Task<IBusControl> Configure(UsingRabbitMqConfig config)
        {
            this.Logger.Info("[*]Starting MassTransit app.......");
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {

                cfg.Message<SendMessage>(x => { x.SetEntityName("Publisher"); });
                cfg.Publish<SendMessage>(p => { p.ExchangeType = "fanout"; });

                cfg.Host($"amqp://{config.host}:{config.port}", h =>
                        {
                            h.Password(config.password);
                            h.Username(config.userName);  
                        });
            });

            await busControl.StartAsync();
            this.Logger.Info("[*]Mass transit configured and started!");
            return busControl;
        }
    }
}
