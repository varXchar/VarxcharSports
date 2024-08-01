using Confluent.Kafka;
using Microsoft.Extensions.Options;
using SportsDataScraperService.Configuration;

namespace SportsDataScraperService.Services;

public interface IMessageBrokerService
{
    Task ProduceAsync();
}

public class MessageBrokerService : IMessageBrokerService
{
    private readonly ILogger<MessageBrokerService> _logger;
    private readonly ApplicationConfiguration _config;

    public MessageBrokerService(ILogger<MessageBrokerService> logger, IOptions<ApplicationConfiguration> config)
    {
        _logger = logger;
        _config = config.Value;
    }

    public async Task ProduceAsync()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };
        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            try
            {
                var topic = "test-topic";
                var message = new Message<Null, string> { Value = "Hello, Kafka!" };
                var dr = await producer.ProduceAsync(topic, message);

                _logger.LogInformation("Delivered {value}", dr.Message.Value);
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError(e, "Delivery failed: {reason}", e.Error.Reason);
            }
        }
    }
}
