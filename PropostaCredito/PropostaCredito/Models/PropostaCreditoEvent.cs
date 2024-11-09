namespace PropostaCredito.Models
{
    public class PropostaCreditoEvent
    {
        public string EventType { get; set; } = string.Empty;
        public Guid ClienteId { get; set; }
        public decimal ValorSolicitado { get; set; }
        public int Parcelas { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}