using System.ComponentModel.DataAnnotations;

namespace ConferenciaFisica.Application.ViewModels
{
    public class SaidaCaminhaoViewModel
    {
        public int? PatioId { get; set; }
        public int? PreRegistroId { get; set; }

        [Display(Name = "Nº Protocolo")]
        public string? ProtocoloPesquisa { get; set; }

        [Display(Name = "Ano Protocolo")]
        public string? AnoProtocoloPesquisa { get { return DateTime.Now.Year.ToString(); } }

        [Display(Name = "Placa")]
        public string? PlacaPesquisa { get; set; }

        [Display(Name = "Placa Carreta")]
        public string? PlacaCarretaPesquisa { get; set; }

        [Display(Name = "Protocolo")]
        public string? Protocolo { get; set; }

        public string? Placa { get; set; }

        [Display(Name = "Placa Carreta")]
        public string? PlacaCarreta { get; set; }

        [Display(Name = "Ticket Entrada")]
        public string? Ticket { get; set; }

        [Display(Name = "Peso Bruto")]
        public int? PesoBruto { get; set; }

        public bool? GateIn { get; set; }

        public bool? GateOut { get; set; }
        public int? Patio { get; set; } = 1;



        [Display(Name = "Finalidade")]
        public string? FinalidadeId { get; set; }

        public int? PatioDestinoId { get; set; }
    }

}
