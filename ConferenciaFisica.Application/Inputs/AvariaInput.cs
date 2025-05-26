using ConferenciaFisica.Domain.Entities;
using System.Text.Json.Serialization;

namespace ConferenciaFisica.Application.Inputs
{
    public class AvariaInput
    {
        [JsonPropertyName("talieId")]
        public int? TalieId { get; set; }


        [JsonPropertyName("local")]
        public int Local { get; set; }


        [JsonPropertyName("divergencia")]
        public bool Divergencia { get; set; }


        [JsonPropertyName("quantidadeAvariada")]
        public int QuantidadeAvariada { get; set; }


        [JsonPropertyName("pesoAvariado")]
        public decimal PesoAvariado { get; set; }


        [JsonPropertyName("observacao")]
        public string Observacao { get; set; }

        [JsonPropertyName("tiposAvarias")]
        public List<TiposAvarias> TiposAvarias { get; set; }
    }
}
