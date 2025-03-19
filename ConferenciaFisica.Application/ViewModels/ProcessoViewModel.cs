namespace ConferenciaFisica.Application.ViewModels
{
    public class ProcessoViewModel
    {
        public int TalieId { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public int Type { get; set; }
        public string TypeDescription { get; set; }
        public string ImagemBase64 { get; set; }
    }
}
