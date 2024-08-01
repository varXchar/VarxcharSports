
using Confluent.Kafka;

namespace SportsScoresService.Service;

public class MessageConsumerService : BackgroundService
{
    private readonly ILogger<MessageConsumerService> _log;
    private readonly IConsumer<Ignore, string> _consumer;

    public MessageConsumerService(ILogger<MessageConsumerService> log)
    {
        _log = log;

        var config = new ConsumerConfig
        {
            GroupId = "test-consumer-group",
            BootstrapServers = "localhost:9092",
            // Note: The AutoOffsetReset property determines the start offset in the event
            // there are not yet any committed offsets for the consumer group for the
            // topic/partitions of interest. By default, offsets are committed
            // automatically, so in this example, consumption will only start from the
            // earliest message in the topic 'test-topic' the first time you run the program.
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => StartConsumerLoop(cancellationToken), cancellationToken);
    }

    private void StartConsumerLoop(CancellationToken cancellationToken)
    {
        _consumer.Subscribe("test-topic");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var cr = _consumer.Consume(cancellationToken);

                cr.

                // Handle message...
                Console.WriteLine($"{cr.Message.Key}: {cr.Message.Value}ms");
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException e)
            {
                // Consumer errors should generally be ignored (or logged) unless fatal.
                Console.WriteLine($"Consume error: {e.Error.Reason}");

                if (e.Error.IsFatal)
                {
                    // https://github.com/edenhill/librdkafka/blob/master/INTRODUCTION.md#fatal-consumer-errors
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: {e}");
                break;
            }
        }
    }

    public override void Dispose()
    {
        _consumer.Dispose();
        base.Dispose();
    }
}
