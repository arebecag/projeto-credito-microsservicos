namespace CartaoCredito.Models
{
    public class CartaoCreditoEvent
    {
        public string EventType { get; set; } = "CartaoCriado";
        public Guid ClienteId { get; set; }
        public string NumeroCartao { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }
}