using CartaoCredito.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CartaoCredito.Consumers
{
    public class PropostaAprovadaConsumer
    {
        private readonly string _hostName = "localhost";
        private readonly int _port = 5672;
        private readonly string _userName = "guest";
        private readonly string _password = "guest";

        public void StartConsuming()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Port = _port,
                UserName = _userName,
                Password = _password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "proposta_aprovada_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "proposta_aprovada_queue", exchange: "proposta_events", routingKey: "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var propostaAprovadaEvent = JsonSerializer.Deserialize<PropostaAprovadaEvent>(message);

                if (propostaAprovadaEvent != null && propostaAprovadaEvent.ClienteId != Guid.Empty)
                {
                    Console.WriteLine($"[Cartão de Crédito] Recebida proposta aprovada para o cliente {propostaAprovadaEvent.ClienteId} com valor aprovado de {propostaAprovadaEvent.ValorAprovado}.");

                    EmitirCartoes(propostaAprovadaEvent);
                }
                else
                {
                    Console.WriteLine("[Cartão de Crédito] Dados inválidos ou ClienteId ausente na proposta aprovada recebida.");
                }
            };

            channel.BasicConsume(queue: "proposta_aprovada_queue", autoAck: true, consumer: consumer);

            Console.WriteLine("Consumidor de Proposta Aprovada iniciado. Pressione [enter] para sair...");
            Console.ReadLine();
        }

        private void EmitirCartoes(PropostaAprovadaEvent proposta)
        {
            int numeroDeCartoes = proposta.ValorAprovado >= 5000 ? 2 : 1;

            for (int i = 1; i <= numeroDeCartoes; i++)
            {
                var cartao = new CartaoDeCredito
                {
                    ClienteId = proposta.ClienteId,
                    NumeroCartao = GerarNumeroCartao(),
                    DataEmissao = DateTime.UtcNow,
                    Validade = DateTime.UtcNow.AddYears(3),
                    LimiteCredito = proposta.ValorAprovado / numeroDeCartoes
                };

                Console.WriteLine($"[Cartão de Crédito] Cartão emitido com sucesso! Número do Cartão: {cartao.NumeroCartao}, Limite: {cartao.LimiteCredito}");
            }
        }

        private string GerarNumeroCartao()
        {
            var random = new Random();
            return string.Concat(
                random.Next(1000, 9999).ToString(),
                random.Next(1000, 9999).ToString(),
                random.Next(1000, 9999).ToString(),
                random.Next(1000, 9999).ToString()
            );
        }
    }
}
