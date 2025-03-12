namespace ConferenciaFisica.Domain.Entities.DescargaExportacao
{
    public class Talie
    {
        public int Id { get; set; }
        public string Inicio { get; set; }
        public string Termino { get; set; }
        public int Conferente { get; set; }
        public int Equipe { get; set; }
        public string Operacao { get; set; }
        public string Observacao { get; set; }
        public List<TalieItem> TalieItem { get; set; }
    }
}
