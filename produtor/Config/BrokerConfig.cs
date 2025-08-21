
using Confluent.Kafka;
namespace Produtor.Config
{
    public class BrokerConfig 
    {
        public static ProducerConfig GetConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
        }
    }
}
