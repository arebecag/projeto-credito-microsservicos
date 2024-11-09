using CartaoCredito.Models;
using System;

namespace CartaoCredito.Services
{
    public interface ICartaoCreditoService
    {
        CartaoDeCredito EmitirCartao(Guid clienteId, decimal limiteCredito);
    }
}
