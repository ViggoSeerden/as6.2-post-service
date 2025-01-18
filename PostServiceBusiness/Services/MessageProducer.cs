using System.Text;
using RabbitMQ.Client;

namespace PostServiceBusiness.Services;

public class MessageProducer
{
    private readonly IChannel _channel;

    public MessageProducer(IChannel channel)
    {
        _channel = channel;
    }
    
    public async void SendMessage(string routingKey, string content)
    {
        var body = Encoding.UTF8.GetBytes(content);

        await _channel.BasicPublishAsync(exchange: "osso-exchange",
            routingKey: routingKey,
            body: body);
        
        Console.WriteLine($" [x] Sent {content} with routing key {routingKey}");
    }
}