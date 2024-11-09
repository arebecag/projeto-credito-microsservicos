using PropostaCredito.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PropostaCredito.Consumers
{
    public class ClienteCadastradoConsumer
    {
        private readonly string _hostName = "localhost";
        private readonly int _port = 5672;
        private readonly string _userName = "guest";
        private readonly string _password = "guest";

        public void StartConsuming()
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

            channel.QueueDeclare(queue: "cliente_cadastrado", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var clienteEvent = JsonSerializer.Deserialize<ClienteCadastradoEvent>(message);
                Console.WriteLine($"[Proposta de Crédito] Processando proposta para o cliente: {clienteEvent.Nome} com ClienteId: {clienteEvent.ClienteId}");

                // Lógica de análise de crédito
                bool aprovado = AnaliseCredito(clienteEvent, out string motivoRejeicao);

                if (aprovado)
                {
                    Console.WriteLine($"[Proposta de Crédito] Proposta aprovada para o cliente: {clienteEvent.Nome}");
                    PublishPropostaAprovada(clienteEvent, channel);
                }
                else
                {
                    Console.WriteLine($"[Proposta de Crédito] Proposta rejeitada para o cliente: {clienteEvent.Nome} - Motivo: {motivoRejeicao}");
                    PublishPropostaRejeitada(clienteEvent, motivoRejeicao, channel);
                }
            };

            channel.BasicConsume(queue: "cliente_cadastrado", autoAck: true, consumer: consumer);

            Console.WriteLine("Consumidor de Cliente Cadastrado para Proposta de Crédito iniciado. Pressione [enter] para sair...");
            Console.ReadLine();
        }

        private bool AnaliseCredito(ClienteCadastradoEvent cliente, out string motivoRejeicao)
        {
            int idade = CalcularIdade(cliente.DataNascimento);

            if (idade < 18)
            {
                motivoRejeicao = "Cliente menor de idade.";
                return false;
            }

            if (cliente.RendaMensal < 2000)
            {
                motivoRejeicao = "Renda mensal insuficiente para aprovação.";
                return false;
            }

            if (cliente.ScoreDeCredito < 600)
            {
                motivoRejeicao = "Score de crédito abaixo do necessário para aprovação.";
                return false;
            }

            motivoRejeicao = string.Empty;
            return true;
        }

        private int CalcularIdade(DateTime dataNascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - dataNascimento.Year;
            if (dataNascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }

        private void PublishPropostaAprovada(ClienteCadastradoEvent cliente, IModel channel)
        {
            var propostaAprovadaEvent = new PropostaAprovadaEvent
            {
                ClienteId = cliente.ClienteId,
                Nome = cliente.Nome,
                ValorAprovado = cliente.RendaMensal * 0.3m,
                Parcelas = 12,
                DataAprovacao = DateTime.UtcNow
            };

            var message = JsonSerializer.Serialize(propostaAprovadaEvent);
            var body = Encoding.UTF8.GetBytes(message);

            channel.QueueDeclare(queue: "proposta_aprovada_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicPublish(exchange: "", routingKey: "proposta_aprovada_queue", basicProperties: null, body: body);
            Console.WriteLine($"Evento PropostaAprovada publicado com ClienteId: {propostaAprovadaEvent.ClienteId}");
        }

        private void PublishPropostaRejeitada(ClienteCadastradoEvent cliente, string motivoRejeicao, IModel channel)
        {
            var propostaRejeitadaEvent = new PropostaRejeitadaEvent
            {
                ClienteId = cliente.ClienteId,
                MotivoRejeicao = motivoRejeicao,
                DataRejeicao = DateTime.UtcNow
            };

            var message = JsonSerializer.Serialize(propostaRejeitadaEvent);
            var body = Encoding.UTF8.GetBytes(message);

            channel.QueueDeclare(queue: "proposta_rejeitada_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicPublish(exchange: "", routingKey: "proposta_rejeitada_queue", basicProperties: null, body: body);
            Console.WriteLine($"Evento PropostaRejeitada publicado com ClienteId: {propostaRejeitadaEvent.ClienteId}");
        }
    }
}
