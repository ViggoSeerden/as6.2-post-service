using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PostServiceBusiness.Services;

public class MessageReceiver : IHostedService
{
    private static ConcurrentDictionary<string, string> _consumedMessage = new();

    private readonly IChannel _channel;
    private readonly IServiceScopeFactory _scopeFactory;

    public MessageReceiver(IServiceScopeFactory scopeFactory, IChannel channel)
    {
        _scopeFactory = scopeFactory;
        _channel = channel;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Declare a topic exchange
        await _channel.ExchangeDeclareAsync(exchange: "osso-exchange", type: ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: cancellationToken);

        // Declare a queue and bind it to the exchange with a routing key pattern
        var queueDeclareResult = await _channel.QueueDeclareAsync(queue: "", durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
        var queueName = queueDeclareResult.QueueName;
        
        await _channel.QueueBindAsync(queue: queueName, exchange: "osso-exchange", routingKey: "user.response.*", cancellationToken: cancellationToken); //request id
        await _channel.QueueBindAsync(queue: queueName, exchange: "osso-exchange",
            routingKey: "post.request.*.*", cancellationToken: cancellationToken); //request id + post id
        Console.WriteLine(" [*] Now listening to messages in osso-exchange'");
        AsyncEventingBasicConsumer consumer = new(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received '{ea.RoutingKey}':'{message}'");

                var routingKeyParts =
                    ea.RoutingKey.Split('.'); // 0 is topic, 1 is request/response, 2 is requestId, 3 is UserId
                var routingKeyBase = $"{routingKeyParts[0]}.{routingKeyParts[1]}";

                using (var scope = _scopeFactory.CreateScope())
                {
                    var postService = scope.ServiceProvider.GetRequiredService<PostService>();
                    var sender = scope.ServiceProvider.GetRequiredService<MessageProducer>();

                    switch (routingKeyBase)
                    {
                        case "user.response":
                            _consumedMessage[routingKeyParts[2]] = message;
                            break;
                        case "post.request":
                            var post = postService.GetPostByIdAsync(new Guid(routingKeyParts[3]));
                            sender.SendMessage($"post.response.{routingKeyParts[2]}", JsonSerializer.Serialize(post));
                            break;
                    }
                }
                
                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: cancellationToken);
                
            }
            catch (Exception ex)
            {
                await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: cancellationToken);
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        };
        await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public static string? GetConsumedMessage(string requestId)
    {
        _consumedMessage.TryGetValue(requestId, out var result);
        return result; // Return null if not found
    }
}