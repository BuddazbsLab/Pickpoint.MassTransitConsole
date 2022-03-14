using MassTransit;
using Message;
using Topshelf;


{
    return (int)HostFactory.Run(cfg => cfg.Service(x => new EventConsumerServiceP()));
}



internal class EventConsumerServiceP :
            ServiceControl
{
    IBusControl _bus;

    public bool Start(HostControl hostControl)
    {
        _bus = ConfigureBus();
        _bus.Start(TimeSpan.FromSeconds(10));

        return true;
    }

    public bool Stop(HostControl hostControl)
    {
        _bus?.Stop(TimeSpan.FromSeconds(30));

        return true;
    }

    IBusControl ConfigureBus()
    {
        return Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.ReceiveEndpoint("event-listener", e =>
            {
                e.Consumer<EventConsumer>();
            });
        });
    }
}

class EventConsumer :
    IConsumer<IValueEntered>
{
    public async Task Consume(ConsumeContext<IValueEntered> context)
    {
        Console.WriteLine("Value: {0}", context.Message.Text);
        Console.ReadLine();
    }
}
