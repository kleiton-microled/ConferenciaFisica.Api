using System.Text.Json.Serialization;

namespace ConferenciaFisica.Application.Inputs
{
    public record LacreConferenciaInput
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("idConferencia")]
        public int IdConferencia { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("tipo")]
        public int Tipo { get; set; }

        [JsonPropertyName("lacreFechamento")]
        public string? LacreFechamento { get; set; }

    }
}
