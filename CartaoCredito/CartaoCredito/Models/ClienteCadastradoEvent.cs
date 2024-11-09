namespace CartaoCredito.Models
{
    public class ClienteCadastradoEvent
    {
        public string EventType { get; set; } = string.Empty;
        public Guid ClienteId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public decimal RendaMensal { get; set; }
        public int ScoreDeCredito { get; set; }
    }
}
