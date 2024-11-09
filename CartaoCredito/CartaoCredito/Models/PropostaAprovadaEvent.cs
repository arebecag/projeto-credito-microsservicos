namespace CartaoCredito.Models
{
    public class PropostaAprovadaEvent
    {
        public Guid ClienteId { get; set; }
        public decimal ValorAprovado { get; set; }
        public int Parcelas { get; set; }
        public DateTime DataAprovacao { get; set; }
    }
}
