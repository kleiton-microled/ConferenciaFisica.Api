namespace ConferenciaFisica.Domain.Entities
{
    public class Conferencia
    {
        public long Id { get; set; }
        public string NumeroBl { get; set; }
        public string Viagem { get; set; }
        public DateTime DataPrevista { get; set; }
        public string MotivoAbertura { get; set; }
        public string Embalagem { get; set; }
        public int Quantidade { get; set; }
        public string Cntr { get; set; } //IdConteiner
        public DateTime? Inicio { get; set; }
        public DateTime? Termino { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCliente { get; set; }
        public int? QtdeAvariada { get; set; }
        public string ObsAvaria { get; set; }
        public int? QuantidadeDivergente { get; set; }
        public string DivergenciaQualificacao { get; set; }
        public string ObsDivergencia { get; set; }
        public int RetiradaAmostra { get; set; }
        public List<Lacre> Lacres { get; set; } = new();
        public string TipoCarga { get; set; }
        public int? QuantidadeRepresentantes { get; set; }
        public int? QuantidadeAjudantes { get; set; }
        public int? QuantidadeOperadores { get; set; }
        public int? QuantidadeVolumesDivergentes { get; set; }
    }

}
