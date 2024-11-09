using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using PropostaCredito.Models;

namespace PropostaCredito.Messaging
{
    public class RabbitMQMessageBus
    {
        private readonly string _hostName = "localhost";

        public void PublishPropostaAprovada(PropostaAprovadaEvent propostaAprovadaEvent)
        {
            PublishEvent("proposta_events", "proposta_aprovada_queue", propostaAprovadaEvent);
        }

        public void PublishPropostaRejeitada(PropostaRejeitadaEvent propostaRejeitadaEvent)
        {
            PublishEvent("proposta_events", "proposta_rejeitada_queue", propostaRejeitadaEvent);
        }

        private void PublishEvent(string exchangeName, string routingKey, object eventMessage)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declara um exchange do tipo 'fanout'
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

            channel.QueueDeclare(queue: routingKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: routingKey, exchange: exchangeName, routingKey: "");

            var message = JsonSerializer.Serialize(eventMessage);
            var body = Encoding.UTF8.GetBytes(message);

            // Publica a mensagem no exchange com o routing key
            channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);
            Console.WriteLine($"Mensagem publicada no exchange '{exchangeName}' com routing key '{routingKey}'");
        }
    }
}
