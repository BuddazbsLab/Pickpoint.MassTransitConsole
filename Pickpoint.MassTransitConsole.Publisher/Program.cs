using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Pickpoint.MassTransitConsole.Publisher;
using Common;
using Pickpoint.MassTransitConsole.Publisher.SizeTextSend;

var configProvider = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).Build();

LogManager.Configuration = new NLogLoggingConfiguration(configProvider.GetSection("NLog"));
var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();

Settings settings = new Settings(configProvider);
var configRMQ = await settings.GetSettingsAppRMQ();
var configSendMessage = await settings.SendParams();





var trafficSizeText = configSendMessage.trafficSendTextInBytes;
var myAlphabet = configSendMessage.alphabet; 
var sendTrafficText = GenerateString.generateStringSize(trafficSizeText, myAlphabet);

var sendingLogic = configSendMessage.sendingLogic;
var packageSize = configSendMessage.packageSize;
var sizeTrafficInMb = configSendMessage.sizeTrafficInMb;

var massTransitConfigurator = new MassTransitConfigurator(logger);
var bus = await massTransitConfigurator.Configure(configRMQ);
var messageSender = new MessageSender(logger, bus, sendTrafficText, sendingLogic, packageSize, sizeTrafficInMb);
var sendConfig = new SendParams 
{
    MessageNumber = configSendMessage.numberMessage,
    SendIntervalSeconds = configSendMessage.messageSendTimeIntervalSeconds,
};
await messageSender.Send(sendConfig);
Console.ReadLine();