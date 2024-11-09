using CadastroClientes.Messaging;
using CadastroClientes.Models;
using CadastroClientes.Services;

namespace CadastroClientes.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IMessageBus _messageBus;

        public ClienteService(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public async Task<Cliente> CadastrarClienteAsync(Cliente cliente)
        {
            cliente.Id = Guid.NewGuid();
           
            _messageBus.PublishClienteCadastrado(cliente);

            return cliente;
        }
    }
}
