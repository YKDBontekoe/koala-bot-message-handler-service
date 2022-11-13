using Infrastructure.Messaging.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Koala.MessageHandlerService;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.UseRabbitMQMessagePublisher(hostContext.Configuration);
                services.UseRabbitMQMessageHandler(hostContext.Configuration);
                services.AddHostedService<MessageHandlerWorker>();
            })
            .UseSerilog((hostContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
            })
            .UseConsoleLifetime()
            .Build();

        await host.RunAsync();
    }
}