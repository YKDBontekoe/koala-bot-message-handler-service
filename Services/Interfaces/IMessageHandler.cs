namespace Koala.MessageHandlerService.Services.Interfaces;

public interface IMessageHandler
{
    public Task InitializeAsync();
    Task CloseQueueAsync();
    
    Task DisposeAsync();
}