namespace Koala.MessageHandlerService.Models;

public class Message
{
    public ulong Id { get; set; }
    public string Content { get; set; }
    public User User { get; set; }
    public Channel Channel { get; set; }
}