using Common;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Pickpoint.MassTransitConsole.Consumer;


var configureProvider = new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).Build();

LogManager.Configuration = new NLogLoggingConfiguration(configureProvider.GetSection("NLog"));
var logger = NLogBuilder.ConfigureNLog(LogManager.Configuration).GetCurrentClassLogger();


Settings settings = new Settings(configureProvider);
var configRMQ = settings.GetSettingsAppRMQ();
var configType = settings.ConfigurationType();



var massTransitConfigurator = new MassTransitConfigurator(logger, settings);
await massTransitConfigurator.MasstransitConfigure(configRMQ);


CreateConfigurationListenerProvaider createConfigurationListenerPrivaider = new(settings, logger);
await createConfigurationListenerPrivaider.ConfigurationListenerProviderAsync(configType);



Console.ReadLine();