﻿using Microsoft.Extensions.Configuration;
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
var config = await settings.GetSettingsApp();

var massTransitConfigurator = new MassTransitConfigurator(logger);
await massTransitConfigurator.MasstransitConfigure(config);


await Task.Delay(50000);
var countGetMessage = EventConsumer.MessageCount;
InfoCountGetMessage infoCountGetMessage = new(logger, countGetMessage);
await infoCountGetMessage.CountMessage(countGetMessage);

Console.ReadLine();