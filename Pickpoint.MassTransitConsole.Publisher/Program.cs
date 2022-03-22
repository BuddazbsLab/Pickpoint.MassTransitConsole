using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Pickpoint.MassTransitConsole.Publisher;
using Common;

var configProvider = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).Build();

LogManager.Configuration = new NLogLoggingConfiguration(configProvider.GetSection("NLog"));
var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();

Settings settings = new Settings(configProvider);
var configRMQ = settings.GetSettingsAppRMQ();
var configType = settings.ConfigurationType();

var massTransitConfigurator = new MassTransitConfigurator(logger);

var bus = await massTransitConfigurator.Configure(configRMQ);

CreateConfigurationProviderFactory createConfigurationProviderFactory = new(settings);
var paramSendMessageProvider = createConfigurationProviderFactory.ConfigurationProvider(configType);

var messageSender = new MessageSender(logger, bus, paramSendMessageProvider);

await messageSender.Send();
Console.ReadLine();