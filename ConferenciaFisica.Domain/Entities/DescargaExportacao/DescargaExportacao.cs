namespace ConferenciaFisica.Domain.Entities.DescargaExportacao
{
    public class DescargaExportacao
    {
        public int Id { get; set; }
        public Talie Talie { get; set; }
        public string Placa { get; set; }
        public string Reserva { get; set; }
        public string Cliente { get; set; }

    }
}
