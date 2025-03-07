namespace ConferenciaFisica.Contracts.Commands
{
    public class LacreConferenciaCommand
    {
        public int? Id { get; set; }
        public int IdConferencia { get; set; }
        public string Numero { get; set; }
        public int Tipo { get; set; }
        public string? LacreFechamento { get; set; }
    }

}
