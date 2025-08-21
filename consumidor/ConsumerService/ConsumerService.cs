using Confluent.Kafka;
using consumidor.config;


namespace consumidor.service
{//hosted : um sevi�o que roda em background, essencial para aplica��es que precisam consumir mensagens de forma cont�nua, como � o caso de um consumidor Kafka.
 
    public class KafkaConsumerService : BackgroundService
    {
        private readonly ConsumerConfiguration _config;
        private readonly IConsumer<Ignore, string> _consumer; // tipo para IConsumer

        public KafkaConsumerService(ConsumerConfiguration config)
        {
            _config = config;
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build(); // Build retorna IConsumer
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) //sobrescreve o m�todo ExecuteAsync da classe BackgroundService
        {
            return Task.Run(() => //tak.run � uma forma de executar uma tarefa em segundo plano por thread de forma assincrona 
            {
                _consumer.Subscribe("pedidos"); // t�pico que vai consumir

                try
                {
                    while (!stoppingToken.IsCancellationRequested) // bloqueia at� receber uma mensagem
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
