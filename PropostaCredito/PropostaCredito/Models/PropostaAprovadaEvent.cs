namespace PropostaCredito.Models
{
    public class PropostaAprovadaEvent
    {
        public Guid ClienteId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal ValorAprovado { get; set; }
        public int Parcelas { get; set; }
        public DateTime DataAprovacao { get; set; }
    }
}
