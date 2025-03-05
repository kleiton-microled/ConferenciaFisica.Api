using System.Text.Json.Serialization;

namespace ConferenciaFisica.Application.Commands
{
    public class ConferenciaFisicaRequest
    {
        public int Id { get; set; }

        [JsonPropertyName("conteiner")]
        public string? Conteiner { get; set; }

        [JsonPropertyName("tipoConferencia")]
        public int Tipo { get; set; }

        [JsonPropertyName("embalagem")]
        public int Embalagem { get; set; }

        [JsonPropertyName("lote")]
        public string? Bl { get; set; }

        [JsonPropertyName("inicioConferencia")]
        public DateTime Inicio { get; set; }

        [JsonPropertyName("fimConferencia")]
        public DateTime Termino { get; set; }

        [JsonPropertyName("cpfConferente")]
        public string? CpfConferente { get; set; }

        [JsonPropertyName("nomeConferente")]
        public string? NomeConferente { get; set; }

        [JsonPropertyName("cpfCliente")]
        public string? CpfCliente { get; set; }

        [JsonPropertyName("nomeCliente")]
        public string? NomeCliente { get; set; }

        [JsonPropertyName("quantidadeDivergente")]
        public int? QtdeDivergente { get; set; }

        [JsonPropertyName("divergenciaQualificacao")]
        public string? DivergenciaQualificacao { get; set; }

        [JsonPropertyName("observacaoDivergencia")]
        public string? ObservacaoDivergencia { get; set; }

        [JsonPropertyName("retiradaAmostra")]
        public int? RetiradaAmostra { get; set; }

        //LACRES
        [JsonPropertyName("conferenciaRemota")]
        public bool? ConferenciaRemota { get; set; }

        [JsonPropertyName("operacao")]
        public string? Operacao { get; set; }

        //modulo?????
        [JsonPropertyName("qtdVolumesDivergentes")]
        public int? QtdeVolumesDivergentes { get; set; }

        [JsonPropertyName("qtdRepresentantes")]
        public int? QtdeRepresentantes { get; set; }

        [JsonPropertyName("qtdAjudantes")]
        public int? QuantidadeAjudantes { get; set; }

        [JsonPropertyName("qtdOperadores")]
        public int? QuantidadeOperadores { get; set; }

        [JsonPropertyName("movimentacao")]
        public int? Movimentacao { get; set; }

        [JsonPropertyName("desunitizacao")]
        public int? Desuniticacao { get; set; }

        [JsonPropertyName("qtdDocumento")]
        public int? QuantidadeDocumentos { get; set; }


    }
}
