using ConferenciaFisica.Domain.Enums;

namespace ConferenciaFisica.Api.Transport.PreRegistro
{
    public class GetDadadosAgendamentoPreRegistroInput
    {
        public int Id { get; set; }

        public string Protocolo { get; set; }

        public string Placa { get; set; }

        public string PlacaCarreta { get; set; }

        public string Ticket { get; set; }

        public DateTime? DataChegada { get; set; }

        public DateTime? DataChegadaDeicPatio { get; set; }

        public bool FlagDeicPatio { get; set; }

        public LocalPatio LocalPatio { get; set; }

        public int IdReg { get; set; }

        public string FinalidadeId { get; set; }
        public int PatioDestinoId { get; set; }
        public string DescPatioDestino { get; set; }
        public string Sistema { get; set; }

        public string Motorista { get; set; }
        public string CNH { get; set; }

    }
}
