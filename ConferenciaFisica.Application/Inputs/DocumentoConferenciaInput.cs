namespace ConferenciaFisica.Application.Inputs
{
    public class DocumentoConferenciaInput
    {
        public int? Id { get; set; }
        public int IdConferencia { get; set; }
        public string Numero { get; set; }
        public int Tipo { get; set; }
    }
}
