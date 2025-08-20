using Confluent.Kafka;
using consumidor.config;


namespace consumidor.service
{//hosted : um seviço que roda em background, essencial para aplicações que precisam consumir mensagens de forma contínua, como é o caso de um consumidor Kafka.
    public class KafkaConsumerService : BackgroundService
    {
        private readonly ConsumerConfiguration _config;

        public KafkaConsumerService(ConsumerConfiguration config)
        {
            _config = config;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) //sobrescreve o método ExecuteAsync da classe BackgroundService
        {
            return Task.Run(() =>
            {
                using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
                consumer.Subscribe("pedidos"); // tópico que vai consumir

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var cr = consumer.Consume(stoppingToken); // bloqueia até receber uma mensagem
                        Console.WriteLine($"Mensagem recebida: {cr.Message.Value}");
                    }
                }
                catch (OperationCanceledException)
                {
                    // Fechar consumer ao encerrar
                    consumer.Close();
                }
            }, stoppingToken);
        }
    }
}
