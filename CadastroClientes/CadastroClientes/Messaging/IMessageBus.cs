using CadastroClientes.Models;

namespace CadastroClientes.Messaging
{
    public interface IMessageBus
    {
        void PublishClienteCadastrado(Cliente cliente);
    }
}
