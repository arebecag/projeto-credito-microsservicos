using CartaoCredito.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CartaoCredito.Messaging
{
    public class RabbitMQMessageBus
    {
        private readonly string _hostName = "localhost";
        private readonly int _port = 5672;
        private readonly string _userName = "guest";
        private readonly string _password = "guest";

        public void PublishCartaoCriado(CartaoCreditoEvent cartaoEvent)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = _port,
                UserName = _userName,
                Password = _password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declara o exchange e a fila para o evento de criação de cartão
            channel.ExchangeDeclare(exchange: "cartao_credito_exchange", type: ExchangeType.Fanout);
            channel.QueueDeclare(queue: "cartao_credito_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "cartao_credito_queue", exchange: "cartao_credito_exchange", routingKey: "");

            // Serializa o evento e publica a mensagem
            var message = JsonSerializer.Serialize(cartaoEvent);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "cartao_credito_exchange", routingKey: "", basicProperties: null, body: body);
            Console.WriteLine("Evento de criação de cartão publicado para o exchange cartao_credito_exchange");
        }
    }
}
