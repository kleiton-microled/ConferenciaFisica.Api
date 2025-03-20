namespace ConferenciaFisica.Contracts.Commands
{
    public class ProcessoCommand
    {

        public int? Id { get; set; }
        public int IdTipoProcesso { get; set; }

        public string ImagemPath { get; set; }

        public string? Descricao { get; set; }

        public string? Observacao { get; set; }
        public int IdTalie { get; set; }
    }
}
