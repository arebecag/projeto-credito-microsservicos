using Microsoft.AspNetCore.Mvc;
using PropostaCredito.Messaging;
using PropostaCredito.Models;

namespace PropostaCredito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropostaCreditoController : ControllerBase
    {
        private readonly RabbitMQMessageBus _messageBus;

        public PropostaCreditoController(RabbitMQMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        [HttpPost("analise")]
        public IActionResult AnalisarProposta([FromBody] PropostaCreditoRequest request)
        {
            Console.WriteLine($"ClienteId recebido na requisição: {request.ClienteId}");

            // Simulação de análise de crédito
            bool aprovado = request.ValorSolicitado <= 10000; // aprova se o valor for menor ou igual a 10.000

            if (aprovado)
            {
                var propostaAprovadaEvent = new PropostaAprovadaEvent
                {
                    ClienteId = request.ClienteId,
                    ValorAprovado = request.ValorSolicitado,
                    Parcelas = request.Parcelas,
                    DataAprovacao = DateTime.UtcNow
                };

                Console.WriteLine($"ClienteId ao publicar o evento de proposta aprovada: {propostaAprovadaEvent.ClienteId}");

                // Publica o evento de proposta aprovada
                _messageBus.PublishPropostaAprovada(propostaAprovadaEvent);

                return Ok(new { Message = "Proposta aprovada e evento enviado com sucesso!" });
            }
            else
            {
                var propostaRejeitadaEvent = new PropostaRejeitadaEvent
                {
                    ClienteId = request.ClienteId,
                    MotivoRejeicao = "Valor solicitado acima do limite",
                    DataRejeicao = DateTime.UtcNow
                };

                Console.WriteLine($"ClienteId ao publicar o evento de proposta rejeitada: {propostaRejeitadaEvent.ClienteId}");

                // Publica o evento de proposta rejeitada
                _messageBus.PublishPropostaRejeitada(propostaRejeitadaEvent);

                return Ok(new { Message = "Proposta rejeitada e evento enviado com sucesso!" });
            }
        }
    }
}
