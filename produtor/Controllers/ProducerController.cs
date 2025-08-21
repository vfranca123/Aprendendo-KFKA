using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Produtor.Config;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;
namespace Produtor.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProducerController : Controller
    {
        private readonly ProducerConfig _producerConfig;
        private readonly IProducer<Null, string> producer;

        public ProducerController(ProducerConfig producerConfig)
        {
            _producerConfig = producerConfig; // corrigido: agora o tipo é ProducerConfig
            producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
        }

        [HttpPost]
        public async Task SendMensage()
        {
            var pedido = new
            {
                Id = 1,
                Produto = "Produto Teste",
                Quantidade = 10,
                Preco = 99.99
            };
            string json = JsonSerializer.Serialize(pedido); // transformando o objeto "pedido" em json

            try
            {
                var result = await producer.ProduceAsync(
                    "pedidos", // nome do tópico
                    new Message<Null, string> { Value = json } // criando a mensagem com o valor em json
                );
                Console.WriteLine(
                    $"Mensagem enviada para o tópico {result.Topic} na partição {result.Partition} com offset {result.Offset}");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Erro ao enviar mensagem: {e.Error.Reason}");
            }
        }
    }
}
