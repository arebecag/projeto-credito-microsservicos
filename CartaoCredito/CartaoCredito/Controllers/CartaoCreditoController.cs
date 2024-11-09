using Microsoft.AspNetCore.Mvc;
using CartaoCredito.Messaging;
using System;
using CartaoCredito.Models;

namespace CartaoCredito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartaoController : ControllerBase
    {
        private readonly RabbitMQMessageBus _messageBus;

        public CartaoController(RabbitMQMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        [HttpPost]
        public IActionResult CriarCartao([FromBody] CartaoRequest request)
        {
            var cartaoEvent = new CartaoCreditoEvent
            {
                ClienteId = request.ClienteId,
                NumeroCartao = "1234-5678-9012-3456",
                DataCriacao = DateTime.UtcNow
            };

            Console.WriteLine($"ClienteId ao criar o evento de cartão: {cartaoEvent.ClienteId}");

            // Publica o evento de criação do cartão
            _messageBus.PublishCartaoCriado(cartaoEvent);

            return Ok(new { Message = "Cartão de crédito criado e evento enviado com sucesso!" });
        }
    }
}
