namespace ConferenciaFisica.Contracts.Commands
{
    public class CadastroAvariaCommand
    {
        public int TalieId { get; set; }
        public int Local { get; set; }
        public bool Divergencia { get; set; }
        public int QuantidadeAvariada { get; set; }
        public decimal PesoAvariado { get; set; }
        public string Observacao { get; set; }

        public List<TiposAvariasCommand> TiposAvarias { get; set; }

        public static CadastroAvariaCommand New(int talieId, int local, bool divergencia, int quantidadeAvariada, decimal pesoAvariado, string observacao, List<TiposAvariasCommand> tiposAvarias)
        {
            var command = new CadastroAvariaCommand();
            command.TalieId = talieId;
            command.Local = local;
            command.Divergencia = divergencia;
            command.QuantidadeAvariada = quantidadeAvariada;
            command.PesoAvariado = pesoAvariado;
            command.Observacao = observacao;
            command.TiposAvarias = tiposAvarias;
            return command;
        }
    }
}
