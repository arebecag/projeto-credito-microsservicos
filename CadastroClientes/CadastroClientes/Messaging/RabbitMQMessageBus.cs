using CadastroClientes.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CadastroClientes.Messaging
{
    public class RabbitMQMessageBus : IMessageBus
    {
        private readonly string _hostName = "localhost";
        private readonly int _port = 5672;
        private readonly string _userName = "guest";
        private readonly string _password = "guest";

        public void PublishClienteCadastrado(Cliente cliente)
        {
            var clienteCadastradoEvent = new ClienteCadastradoEvent
            {
                ClienteId = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                DataNascimento = cliente.DataNascimento,
                RendaMensal = cliente.RendaMensal,
                ScoreDeCredito = cliente.ScoreDeCredito
            };

            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = _port,
                UserName = _userName,
                Password = _password,
                DispatchConsumersAsync = true
            };

            try
            {
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(
                    queue: "cliente_cadastrado",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var message = JsonSerializer.Serialize(clienteCadastradoEvent);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "cliente_cadastrado",
                    basicProperties: null,
                    body: body
                );

                Console.WriteLine($"Mensagem publicada na fila cliente_cadastrado com ClienteId: {clienteCadastradoEvent.ClienteId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao tentar publicar a mensagem: {ex.Message}");
            }
        }
    }
}
