namespace ConferenciaFisica.Domain.Entities.DescargaExportacao
{
    public class DescargaExportacao
    {
        public int Id { get; set; }
        public int Registro { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public string Talie { get; set; }
        public string Placa { get; set; }
        public string Reserva { get; set; }
        public string Cliente { get; set; }
        public string Conferente { get; set; }
        public int Equipe { get; set; }
        public int Operacao { get; set; }
        public List<ItensDescargaExportacao> ItensDescarga { get; set; }

    }
}
