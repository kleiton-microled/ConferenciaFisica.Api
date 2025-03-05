namespace ConferenciaFisica.Contracts.Commands
{
    public class CadastroAdicionalCommand
    {
        public int IdConferencia { get; set; }
        public string Nome { get; set; }

        private string _cpf;
        public string Cpf
        {
            get => _cpf;
            set => _cpf = SanitizeCpf(value);
        }

        public string Qualificacao { get; set; }
        public string Tipo { get; set; }

        private string SanitizeCpf(string cpf)
        {
            return string.IsNullOrEmpty(cpf) ? cpf : cpf.Replace(".", "").Replace("-", "");
        }
    }

}
