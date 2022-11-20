using Azure.Messaging.ServiceBus;
using Koala.MessageHandlerService.Models;
using Koala.MessageHandlerService.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Koala.MessageHandlerService.Services;

public class MessageHandler : IMessageHandler
{
    private readonly IConfiguration _configuration;
    private readonly ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor? _processor;

    public MessageHandler(IConfiguration configuration, ServiceBusClient serviceBusClient)
    {
        _configuration = configuration;
        _serviceBusClient = serviceBusClient;
    }

    public async Task InitializeAsync()
    {
        _processor = _serviceBusClient.CreateProcessor(_configuration["ServiceBus:UserMessagesQueueName"], new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = true,
            MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(15),
            PrefetchCount = 100,
        });
        
        try
        {
            // add handler to process messages
            _processor.ProcessMessageAsync += MessagesHandler;

            // add handler to process any errors
            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task CloseQueueAsync()
    {
        if (_processor != null) await _processor.CloseAsync();
    }

    public async Task DisposeAsync()
    {
        if (_processor != null) await _processor.DisposeAsync();
    }

    private async Task MessagesHandler(ProcessMessageEventArgs args)
    {
        // Process the message.
        var body = args.Message.Body.ToString();
        var message = JsonConvert.DeserializeObject<Message>(body);

        if (message is null) return;

        ServiceBusSender? sender;
        if (message.Content.StartsWith("/"))
        {
            sender = _serviceBusClient.CreateSender(_configuration["ServiceBus:CommandQueueName"]);
            await sender.SendMessageAsync(new ServiceBusMessage(body));
        }

        if (message.User.Id == 316185776021045248)
        {
            sender = _serviceBusClient.CreateSender(_configuration["ServiceBus:SendMessageQueueName"]);
            await sender.SendMessageAsync(new ServiceBusMessage(JsonConvert.SerializeObject(new SendMessage() { ChannelId = message.Channel.Id, IsReaction = true, OriginalMessageId = message.Id, Content = "🤡" })));
        }

        sender = _serviceBusClient.CreateSender(_configuration["ServiceBus:ConsumerQueueName"]);
        await sender.SendMessageAsync(new ServiceBusMessage(body));
    }
    
    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        // Process the error.
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}