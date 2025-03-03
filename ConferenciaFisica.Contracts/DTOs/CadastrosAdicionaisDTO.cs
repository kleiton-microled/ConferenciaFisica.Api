namespace ConferenciaFisica.Contracts.DTOs
{
    public class CadastrosAdicionaisDTO
    {
        public int Id { get; set; }
        public int IdConferencia { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Qualificacao { get; set; }
        public string Tipo { get; set; }
    }
}
