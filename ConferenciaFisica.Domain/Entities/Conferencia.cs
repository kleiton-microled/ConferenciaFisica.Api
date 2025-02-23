namespace ConferenciaFisica.Domain.Entities
{
    public class Conferencia
    {
        public long Id { get; set; }
        public string NumeroBl { get; set; }
        public string NumeroCntr { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Termino { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCliente { get; set; }
        public int? QtdeAvariada { get; set; }
        public string ObsAvaria { get; set; }
        public int? DivergenciaQuantidade { get; set; }
        public string DivergenciaQualificacao { get; set; }
        public string ObsDivergencia { get; set; }
        public bool RetiradaAmostra { get; set; }
        public List<Lacre> Lacres { get; set; } = new();
        public string TipoCarga { get; set; }
        public int? QtdRepresentantes { get; set; }
        public int? QtdAjudantes { get; set; }
        public int? QtdOperadores { get; set; }
    }

}
