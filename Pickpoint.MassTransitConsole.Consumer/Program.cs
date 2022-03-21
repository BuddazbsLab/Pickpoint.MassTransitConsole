using Common;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Pickpoint.MassTransitConsole.Consumer;
using Pickpoint.MassTransitConsole.Consumer.Consume;


var configureProvider = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).Build();

LogManager.Configuration = new NLogLoggingConfiguration(configureProvider.GetSection("NLog"));
var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();


Settings settings = new Settings(configureProvider);
var configRMQ = await settings.GetSettingsAppRMQ();
var configSendMessage = await settings.SendParams();


var massTransitConfigurator = new MassTransitConfigurator(logger);
await massTransitConfigurator.MasstransitConfigure(configRMQ);

if (configSendMessage.sizeTrafficInMb > 0)
{
    var delayReciveMessage = (configSendMessage.sizeTrafficInMb * 36000);
    await Task.Delay(delayReciveMessage);
    var countGetMessage = EventConsumer.MessageCount;
    InfoCountGetMessage infoCountGetMessage = new(logger, countGetMessage);
    await infoCountGetMessage.CountMessage(countGetMessage);
}
else
{
    var delayReciveMessage = (configSendMessage.messageSendTimeIntervalSeconds * 1050);
    await Task.Delay(delayReciveMessage);
    var countGetMessage = EventConsumer.MessageCount;
    InfoCountGetMessage infoCountGetMessage = new(logger, countGetMessage);
    await infoCountGetMessage.CountMessage(countGetMessage);
}


Console.ReadLine();