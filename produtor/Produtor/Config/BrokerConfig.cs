
using Confluent.Kafka;
namespace Produtor.Config
{
    public class BrokerConfig : ProducerConfig
    {
        public string bootstrapServer = "Colocar aqui o enderço de onde o broker esta rodando";
    }
}
