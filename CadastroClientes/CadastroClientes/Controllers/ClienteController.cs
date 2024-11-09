using CadastroClientes.Models;
using CadastroClientes.Services;
using Microsoft.AspNetCore.Mvc;

namespace CadastroClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarCliente([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                return BadRequest("Cliente não pode ser nulo.");
            }

            if (cliente.RendaMensal <= 0)
            {
                return BadRequest("RendaMensal deve ser maior que zero.");
            }

            if (cliente.ScoreDeCredito <= 0 || cliente.ScoreDeCredito > 1000)
            {
                return BadRequest("ScoreDeCredito deve estar entre 1 e 1000.");
            }

            var novoCliente = await _clienteService.CadastrarClienteAsync(cliente);
            return Ok(new { Message = "Cliente cadastrado com sucesso!", Cliente = novoCliente });
        }
    }
}
