using NLog;
using Pickpoint.MassTransitConsole.Consumer;
using Pickpoint.MassTransitConsole.Consumer.Consume;

Settings settings = new Settings();
var logger = LogManager.GetCurrentClassLogger();
await settings.GetSettingsApp();

await Task.Delay(123999 + 90);
var count = EventConsumer.MessageCount;

Console.ReadLine();