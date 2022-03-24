using Common;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Pickpoint.RabbitMQ.Publisher;

var configProvider = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).Build();


LogManager.Configuration = new NLogLoggingConfiguration(configProvider.GetSection("NLog"));
var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();

Settings settings = new Settings(configProvider);
var configRMQ = settings.GetSettingsAppRMQ();
var paramsendRMQ = settings.ConfigurationBasicSendRMQ();


var rabbitMQConfiguration = new RabbitMQConfiguration(logger);
var initRabbitMQ = await rabbitMQConfiguration.InitializeConfigure(configRMQ);

SendMessageRMQ sendMessage = new SendMessageRMQ(logger);
await sendMessage.InitSendMessage(initRabbitMQ, configRMQ, paramsendRMQ);

Console.ReadLine();
