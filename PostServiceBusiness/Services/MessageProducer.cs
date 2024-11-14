using System.Text;
using RabbitMQ.Client;

namespace PostServiceBusiness.Services;

public class MessageProducer
{
    private readonly IModel _channel;

    public MessageProducer()
    {
        var factory = new ConnectionFactory { HostName = Environment.GetEnvironmentVariable("RabbitMQ") ?? "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.QueueDeclare(queue: "send-email",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }
    
    public void SendMessage()
    {
        const string message = "Send an Email";
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: string.Empty,
            routingKey: "send-email",
            basicProperties: null,
            body: body);
        Console.WriteLine($" [x] Sent {message}");
    }
}