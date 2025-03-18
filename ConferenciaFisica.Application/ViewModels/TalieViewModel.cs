using System.Text.Json.Serialization;

namespace ConferenciaFisica.Application.ViewModels
{
    public class TalieViewModel
    {
        public int Id { get; set; }
        public string Inicio { get; set; }
        public string? Termino { get; set; }
        public int Conferente { get; set; }
        public int Equipe { get; set; } = 1;
        public string? Operacao { get; set; } = "1";
        public string? Observacao { get; set; }
        public List<TalieItemViewModel> TalieItem { get; set; }
    }
}
