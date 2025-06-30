namespace ConferenciaFisica.Application.ViewModels
{
    public class DescargaExportacaoViewModel
    {
        public int Registro { get; set; }
        public TalieViewModel? Talie { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Reserva { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public int IdReserva { get; set; }
        public string? NomeConferente { get; set; }
        public int Equipe { get; set; }
        public int Operacao { get; set; }
        public bool IsCrossDocking { get; set; }
        public string? Conteiner { get; set; } = string.Empty;
    }
}
