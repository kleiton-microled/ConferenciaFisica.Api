using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenciaFisica.Domain.Entities.PreRegistro
{
    public class DadosAgendamentoModel
    {
        public int AgendamentoId { get; set; }

        public string Protocolo { get; set; }

        public string Periodo { get; set; }

        public string Motorista { get; set; }

        public string CNH { get; set; }

        public string Placa { get; set; }

        public string PlacaCarreta { get; set; }

        public DateTime PeriodoInicial { get; set; }

        public DateTime PeriodoFinal { get; set; }

        public int Tipo { get; set; }

        public DateTime? DataChegadaPatio { get; set; }

        public DateTime? DataSaidaPatio { get; set; }

        public DateTime? DataChegadaDeicPatio { get; set; }

        public DateTime? DataSaidaDeicPatio { get; set; }

        public string Periodo_Inicial { get; set; }
        public string Periodo_Final { get; set; }

        public string Ticket { get; set; }
        public int Ano { get; set; }
        public string Sistema { get; set; }
    }
}
