using CartaoCredito.Models;

namespace CartaoCredito.Services
{
    public class CartaoCreditoService : ICartaoCreditoService
    {
        private readonly List<CartaoDeCredito> _cartoesEmitidos = new();

        public CartaoDeCredito EmitirCartao(Guid clienteId, decimal limiteCredito)
        {
            var cartao = new CartaoDeCredito
            {
                ClienteId = clienteId,
                NumeroCartao = GerarNumeroCartao(),
                DataEmissao = DateTime.UtcNow,
                Validade = DateTime.UtcNow.AddYears(3),
                LimiteCredito = limiteCredito
            };

            _cartoesEmitidos.Add(cartao);

            Console.WriteLine($"Cartão de crédito emitido para o cliente {clienteId}. Número do cartão: {cartao.NumeroCartao}");
            return cartao;
        }

        private string GerarNumeroCartao()
        {
            var random = new Random();
            return string.Concat(
                random.Next(1000, 9999).ToString(),
                random.Next(1000, 9999).ToString(),
                random.Next(1000, 9999).ToString(),
                random.Next(1000, 9999).ToString());
        }
    }
}
