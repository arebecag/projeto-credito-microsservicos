using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using CartaoCredito.Models;
using CartaoCredito.Services;

namespace CartaoCredito.Consumers
{
    public class ClienteCadastradoConsumer
    {
        private readonly string _hostName = "localhost";
        private readonly int _port = 5672;
        private readonly string _userName = "guest";
        private readonly string _password = "guest";
        private readonly ICartaoCreditoService _cartaoCreditoService;

        public ClienteCadastradoConsumer(ICartaoCreditoService cartaoCreditoService)
        {
            _cartaoCreditoService = cartaoCreditoService;
        }

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

            channel.ExchangeDeclare(exchange: "cliente_events", type: ExchangeType.Fanout, durable: false, autoDelete: false);
            channel.QueueDeclare(queue: "cartao_credito_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: "cartao_credito_queue", exchange: "cliente_events", routingKey: "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var clienteEvent = JsonSerializer.Deserialize<ClienteCadastradoEvent>(message);
                Console.WriteLine($"[Cartão de Crédito] Processando emissão de cartão para o cliente: {clienteEvent.Nome} com ClienteId: {clienteEvent.ClienteId}");

                if (clienteEvent.ScoreDeCredito >= 600 && clienteEvent.RendaMensal >= 2000)
                {
                    var limiteCredito = clienteEvent.RendaMensal * 0.5m;
                    var cartao = _cartaoCreditoService.EmitirCartao(clienteEvent.ClienteId, limiteCredito);
                    Console.WriteLine($"[Cartão de Crédito] Cartão emitido com sucesso para o cliente: {clienteEvent.Nome}. Número do cartão: {cartao.NumeroCartao}");
                }
                else
                {
                    Console.WriteLine($"[Cartão de Crédito] Cliente {clienteEvent.Nome} não atende aos critérios para emissão de cartão.");
                }
            };

            channel.BasicConsume(queue: "cartao_credito_queue", autoAck: true, consumer: consumer);

            Console.WriteLine("Consumidor de Cartão de Crédito iniciado.");
            Console.ReadLine();
        }
    }
}
