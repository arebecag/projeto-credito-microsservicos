namespace PropostaCredito.Models
{
    public class PropostaRejeitadaEvent
    {
        public Guid ClienteId { get; set; }
        public string MotivoRejeicao { get; set; } = string.Empty;
        public DateTime DataRejeicao { get; set; }
    }
}
