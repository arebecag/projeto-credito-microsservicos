namespace CartaoCredito.Models
{
    public class CartaoDeCredito
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClienteId { get; set; }
        public string NumeroCartao { get; set; } = string.Empty;
        public DateTime DataEmissao { get; set; }
        public DateTime Validade { get; set; }
        public decimal LimiteCredito { get; set; }
    }
}
