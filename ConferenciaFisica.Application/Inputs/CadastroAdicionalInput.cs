using System.Text.Json.Serialization;

namespace ConferenciaFisica.Application.Inputs
{
    public record CadastroAdicionalInput
    {
        public int IdConferencia { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Qualificacao { get; set; }
        public string Tipo { get; set; }

    }
}
