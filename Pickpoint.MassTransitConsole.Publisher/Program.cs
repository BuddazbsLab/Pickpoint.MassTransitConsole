using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Pickpoint.MassTransitConsole.Publisher;
using Pickpoint.MassTransitConsole.Publisher.Models;
using Common;
using Pickpoint.MassTransitConsole.Publisher.Traffic;

var configProvider = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).Build();

LogManager.Configuration = new NLogLoggingConfiguration(configProvider.GetSection("NLog"));
var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();

Settings settings = new Settings(configProvider);
var config = await settings.GetSettingsApp();

var trafficSize = config.trafficSendinBytes;
var myAlphabet = config.alphabet; 
var sendTraffic = GenerateString.generateStringSize(trafficSize, myAlphabet);


var massTransitConfigurator = new MassTransitConfigurator(logger);
var bus = await massTransitConfigurator.Configure(config);
var messageSender = new MessageSender(logger, bus, sendTraffic);
var sendConfig = new SendParams 
{
    MessageNumber = config.numberMessage,
    SendIntervalSeconds = config.messageSendTimeIntervalSeconds,
};
await messageSender.Send(sendConfig);
Console.ReadLine();