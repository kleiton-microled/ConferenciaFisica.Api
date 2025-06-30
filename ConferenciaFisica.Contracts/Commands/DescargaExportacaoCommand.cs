using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Contracts.Commands
{
    public class DescargaExportacaoCommand
    {
        public int Registro { get; set; }
        public TalieDTO Talie { get; set; }
        public string Placa { get; set; }
        public string Reserva { get; set; }
        public string Cliente { get; set; }
        public int? IdReserva { get; set; }
        public int IdConferente { get; set; }
        public int? Equipe { get; set; }
        public int? Operacao { get; set; }
        public bool IsCrossDocking { get; set; }
        public string Conteiner { get; set; }

        public static DescargaExportacaoCommand CreateNew(int registro, 
            TalieDTO talie, 
            string placa, 
            string reserva,
            string cliente, int idReserva, int idConferente, int idEquipe, int operacao, bool isCrossDocking, string conteiner)
        {
            var cmd = new DescargaExportacaoCommand();
            cmd.Registro = registro;
            cmd.Talie = talie;
            cmd.Placa = placa;
            cmd.Reserva = reserva;
            cmd.Cliente = cliente;
            cmd.IdReserva = idReserva;
            cmd.IdConferente = idConferente;
            cmd.Equipe = idEquipe;
            cmd.Operacao = operacao;
            cmd.IsCrossDocking = isCrossDocking;
            cmd.Conteiner = conteiner;

            return cmd;
        }

    }
}
