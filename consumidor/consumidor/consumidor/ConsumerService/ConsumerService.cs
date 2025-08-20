using Confluent.Kafka;
using consumidor.config;


namespace consumidor.service
{//hosted : um sevi�o que roda em background, essencial para aplica��es que precisam consumir mensagens de forma cont�nua, como � o caso de um consumidor Kafka.
    public class KafkaConsumerService : BackgroundService
    {
        private readonly ConsumerConfiguration _config;

        public KafkaConsumerService(ConsumerConfiguration config)
        {
            _config = config;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) //sobrescreve o m�todo ExecuteAsync da classe BackgroundService
        {
            return Task.Run(() =>
            {
                using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
                consumer.Subscribe("pedidos"); // t�pico que vai consumir

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var cr = consumer.Consume(stoppingToken); // bloqueia at� receber uma mensagem
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
