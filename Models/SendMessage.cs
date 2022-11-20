namespace Koala.MessageHandlerService.Models;

public class SendMessage
{
    public ulong ChannelId { get; set; }
    public string Content { get; set; }
    public bool IsReaction { get; set; }
    public ulong? OriginalMessageId { get; set; }
}