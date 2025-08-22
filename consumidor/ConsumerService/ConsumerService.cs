using Confluent.Kafka;

namespace consumidor.service
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly ConsumerConfig _config;
        private readonly IConsumer<Ignore, string> _consumer;

        public KafkaConsumerService(ConsumerConfig config)
        {
            _config = config;
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                _consumer.Subscribe("pedidos");

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var cr = _consumer.Consume(stoppingToken);
                        Console.WriteLine($"Mensagem recebida: {cr.Message.Value}");
                    }
                }
                catch (OperationCanceledException)
                {
                    _consumer.Close();
                }
            }, stoppingToken);
        }
    }
}

