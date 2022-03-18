using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Pickpoint.MassTransitConsole.Publisher;
using Pickpoint.MassTransitConsole.Publisher.Models;

var configProvider = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).Build();

LogManager.Configuration = new NLogLoggingConfiguration(configProvider.GetSection("NLog"));
var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();

Settings settings = new Settings(configProvider);
var configS = await settings.GetSettingsApp();

var massTransitConfigurator = new MassTransitConfigurator(logger);
var bus = await massTransitConfigurator.Configure(configS);
var messageSender = new MessageSender(logger, bus);
var sendConfig = new SendParams 
{
    MessageNumber = configS.numberMessage,
    SendIntervalSeconds = 6,
};
await messageSender.Send(sendConfig);
Console.ReadLine();