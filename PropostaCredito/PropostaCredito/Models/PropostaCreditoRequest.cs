namespace PropostaCredito.Models
{
    public class PropostaCreditoRequest
    {
        public Guid ClienteId { get; set; }
        public decimal ValorSolicitado { get; set; }
        public int Parcelas { get; set; }
    }
}