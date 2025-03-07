namespace ConferenciaFisica.Domain.Entities
{
    public class DocumentosConferencia
    {
        public int Id { get; set; }
        public int IdConferencia { get; set; }
        public string Numero { get; set; }
        public int Tipo { get; set; }
        public string TipoDescricao { get; set; }

    }
}
