namespace PropostaCredito.Models
{
    public class ClienteCadastradoEvent
    {
        public Guid ClienteId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public decimal RendaMensal { get; set; }
        public int ScoreDeCredito { get; set; }
    }
}