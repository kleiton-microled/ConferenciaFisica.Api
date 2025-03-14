namespace ConferenciaFisica.Application.ViewModels
{
    public class DescargaExportacaoViewModel
    {
        public int Registro { get; set; }
        public TalieViewModel? Talie { get; set; }
        public string Placa { get; set; }
        public string Reserva { get; set; }
        public string Cliente { get; set; }
    }
}
