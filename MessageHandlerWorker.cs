using Infrastructure.Messaging.Handlers.Interfaces;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Koala.MessageHandlerService;

public class MessageHandlerWorker : IHostedService, IMessageHandlerCallback 
{
    private readonly IMessageHandler _messageHandler;

    public MessageHandlerWorker(IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _messageHandler.Start(this);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _messageHandler.Stop();
        return Task.CompletedTask;
    }

    public Task<bool> HandleMessageAsync(string messageType, string message)
    {
        try
        {
            switch (messageType)
            {
                case "MESSAGE_RECEIVED":
                    Console.WriteLine(message);
                    break;
                default:
                    // Handle the message
                    Console.WriteLine($"Message type {messageType} not handled and has been ignored.");
                    break;
            }
        } catch (Exception ex)
        {
            Log.Error("Error handling message: {ExMessage}", ex.Message);
        }

        return Task.FromResult(true);
    }
}