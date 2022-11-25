namespace Koala.MessageHandlerService.Options;

public class ServiceBusOptions
{
    public const string ServiceBus = "ServiceBus";
    
    public string ConnectionString { get; set; } = string.Empty;
    public string UserMessagesQueueName { get; set; } = string.Empty;
    public string ConsumerQueueName { get; set; } = string.Empty;
    public string SendMessageQueueName { get; set; } = string.Empty;
    public string CommandQueueName { get; set; } = string.Empty;
}