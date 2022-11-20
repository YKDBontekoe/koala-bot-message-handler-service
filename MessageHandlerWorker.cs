using Koala.MessageHandlerService.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Koala.MessageHandlerService;

public class MessageHandlerWorker : IHostedService
{
    private readonly IMessageHandler _messageHandler;

    public MessageHandlerWorker(IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _messageHandler.InitializeAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _messageHandler.DisposeAsync()!;
        await _messageHandler.CloseQueueAsync()!;
    }
}