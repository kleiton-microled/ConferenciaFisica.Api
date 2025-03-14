using ConferenciaFisica.Domain.Entities;
using System.Text.Json.Serialization;

namespace ConferenciaFisica.Application.Inputs
{
    public class AvariaConferenciaInput
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("idConferencia")]
        public int IdConferencia { get; set; }

        [JsonPropertyName("quantidadeAvariada")]
        public int QuantidadeAvariada { get; set; }

        [JsonPropertyName("pesoAvariado")]
        public decimal PesoAvariado { get; set; }

        [JsonPropertyName("idEmbalagem")]
        public int IdEmbalagem { get; set; }

        [JsonPropertyName("conteiner")]
        public string Conteiner { get; set; }

        [JsonPropertyName("observacao")]
        public string Observacao { get; set; }

        [JsonPropertyName("tiposAvarias")]
        public List<TiposAvarias> TiposAvarias { get; set; }

    }
}
