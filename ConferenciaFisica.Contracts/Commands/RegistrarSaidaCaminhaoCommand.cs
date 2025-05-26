using System.Data;

namespace ConferenciaFisica.Contracts.Commands
{
    public class RegistrarSaidaCaminhaoCommand
    {
        public string Name { get; set; } = "PreRegistroId";
        public int PreRegistroId { get; set; }
        public int Value { get; set; }
        public ParameterDirection Direction { get; set; }
    }
}
