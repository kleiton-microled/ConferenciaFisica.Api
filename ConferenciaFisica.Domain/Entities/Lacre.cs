namespace ConferenciaFisica.Domain.Entities
{
    public class Lacre
    {
        public int Id { get; set; }
        public int IdConferencia { get; set; }
        public string Numero { get; set; }
        public int Tipo { get; set; }
        public string DescricaoTipo { get; set; }
        public string LacreFechamento { get; set; }
    }
}
