namespace Pickpoint.MassTransitConsole.Publisher
{
    public interface ISendConfigurationProvider
    {
        InnerSendConfig GetConfig();
    }
}
