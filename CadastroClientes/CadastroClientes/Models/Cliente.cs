namespace CadastroClientes.Models
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public decimal RendaMensal { get; set; }
        public int ScoreDeCredito { get; set; }
        public bool Ativo { get; set; } = true;
    }

}
