using Confluent.Kafka;
using consumidor.config;


namespace consumidor.service
{//hosted : um seviço que roda em background, essencial para aplicações que precisam consumir mensagens de forma contínua, como é o caso de um consumidor Kafka.
 
    public class KafkaConsumerService : BackgroundService
    {
        private readonly ConsumerConfiguration _config;
        private readonly IConsumer<Ignore, string> _consumer; // tipo para IConsumer

        public KafkaConsumerService(ConsumerConfiguration config)
        {
            _config = config;
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build(); // Build retorna IConsumer
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) //sobrescreve o método ExecuteAsync da classe BackgroundService
        {
            return Task.Run(() => //tak.run é uma forma de executar uma tarefa em segundo plano por thread de forma assincrona 
            {
                _consumer.Subscribe("pedidos"); // tópico que vai consumir

                try
                {
                    while (!stoppingToken.IsCancellationRequested) // bloqueia até receber uma mensagem
                    {
                        var cr = _consumer.Consume(stoppingToken);
                        Console.WriteLine($"Mensagem recebida: {cr.Message.Value}");
                    }
                }
                catch (OperationCanceledException)
                {
                    // Fechar consumer ao encerrar
                    _consumer.Close();
                }
            }, stoppingToken);
        }
    }
}
