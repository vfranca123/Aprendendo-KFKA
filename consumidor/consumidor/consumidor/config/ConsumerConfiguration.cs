using Confluent.Kafka;

namespace consumidor.config
{
    public class ConsumerConfiguration : ConsumerConfig
    {
        public ConsumerConfiguration()
        {
            BootstrapServers = "localhost:9092";
            GroupId = "grupo1";
            AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Earliest;
        }
    }
}
