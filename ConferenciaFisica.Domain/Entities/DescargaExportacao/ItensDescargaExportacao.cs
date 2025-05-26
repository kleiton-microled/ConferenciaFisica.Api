namespace ConferenciaFisica.Domain.Entities.DescargaExportacao
{
    public class ItensDescargaExportacao
    {
        public int Id { get; set; }
        public string NotaFisca { get; set; }
        public int Item { get; set; }
        public int Embalagem { get; set; }
        public int QuantidadeNf { get; set; }
        public int QuantidadeDescarga { get; set; }
    }
}
