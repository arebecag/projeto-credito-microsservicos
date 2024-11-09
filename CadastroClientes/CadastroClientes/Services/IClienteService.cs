using CadastroClientes.Models;

namespace CadastroClientes.Services
{
    public interface IClienteService
    {
        Task<Cliente> CadastrarClienteAsync(Cliente cliente);
    }
}
