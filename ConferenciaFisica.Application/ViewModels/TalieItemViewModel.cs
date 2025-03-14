using System.Text.Json.Serialization;

namespace ConferenciaFisica.Application.ViewModels
{
    public class TalieItemViewModel
    {
        public int Id { get; set; }
        public string? NotaFiscal { get; set; }
        public string? Embalagem { get; set; }
        public int? QuantidadeNf { get; set; }
        public int QuantidadeDescarga { get; set; }
        public string? Remonte { get; set; }
        public string? Fumigacao { get; set; }
        public int? RegistroCsId { get; set; }
        public string? Descricao { get; set; }
        public int? Quantidade { get; set; }
        public int? PatioId { get; set; }
        public decimal? Comprimento { get; set; }
        public decimal? Largura { get; set; }
        public decimal? Altura { get; set; }
        public decimal? Peso { get; set; }
        public bool? Fragil { get; set; }
        public bool? Madeira { get; set; }
        public bool? Avariado { get; set; }
        public string? Yard { get; set; }
        public int? NotaFiscalId { get; set; }
        [JsonPropertyName("imo")]
        public string? IMO { get; set; }
        public string? IMO2 { get; set; }
        public string? IMO3 { get; set; }
        public string? IMO4 { get; set; }
        public string? IMO5 { get; set; }
        public string? UNO { get; set; }
        public string? UNO2 { get; set; }
        public string? UNO3 { get; set; }
        public string? UNO4 { get; set; }
        public string? UNO5 { get; set; }
        public string? EmbalagemSigla { get; set; }
        public int? CodigoEmbalagem { get; set; }
        public int QtdDescarga { get; set; }
        public string? Resumo { get; set; }

    }
}
