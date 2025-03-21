namespace ConferenciaFisica.Application.ViewModels
{
    public class ProcessoViewModel
    {
        public int? Id { get; set; }
        public int IdTipoFoto { get; set; }
        public int IdTipoProcesso { get; set; }
        public int? TalieId { get; set; }
        public string? ContainerId { get; set; }
        public string? ImagemPath { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public string ImagemBase64 { get; set; }
       
    }
}
