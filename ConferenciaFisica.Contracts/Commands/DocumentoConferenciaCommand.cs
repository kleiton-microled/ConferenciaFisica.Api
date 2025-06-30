namespace ConferenciaFisica.Contracts.Commands
{
    public class DocumentoConferenciaCommand
    {
        public int? Id { get; set; }
        public int IdConferencia { get; set; }
        public string Numero { get; set; }
        public int Tipo { get; set; }
        public int Quantidade { get; set; }

    }
}
