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

        public static DescargaExportacaoCommand CreateNew(int registro, TalieDTO talie, string placa, string reserva, string cliente)
        {
            var cmd = new DescargaExportacaoCommand();
            cmd.Registro = registro;
            cmd.Talie = talie;
            cmd.Placa = placa;
            cmd.Reserva = reserva;
            cmd.Cliente = cliente;

            return cmd;
        }

    }
}
