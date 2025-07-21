namespace ConferenciaFisica.Domain.Entities
{
    public class Conferencia
    {
        public long Id { get; set; }
        public int Tipo { get; set; }
        public string Bl { get; set; }
        public string Viagem { get; set; }
        public DateTime DataPrevista { get; set; }
        public string MotivoAbertura { get; set; }
        public string Embalagem { get; set; }
        public int Quantidade { get; set; }
        public int? Cntr { get; set; } //IdConteiner
        public string NumeroConteiner { get; set; } //NumeroConteiner
        public DateTime? Inicio { get; set; }
        public DateTime? Termino { get; set; }
        public string NomeCliente { get; set; }
        public string CpfCliente { get; set; }
        public string TelefoneConferente { get; set; }
        public int? QtdeAvariada { get; set; }
        public string ObsAvaria { get; set; }
        public int? QuantidadeDivergente { get; set; }
        public bool DivergenciaQualificacao { get; set; }
        public string ObservacaoDivergencias { get; set; }
        public int RetiradaAmostra { get; set; }
        public List<Lacre> Lacres { get; set; } = new();
        public string TipoCarga { get; set; }
        public int? QuantidadeRepresentantes { get; set; }
        public int? QuantidadeAjudantes { get; set; }
        public int? QuantidadeOperadores { get; set; }
        public int? QuantidadeVolumesDivergentes { get; set; }
        public int? Movimentacao { get; set; }
        public int? Desunitizacao { get; set; }
    }

}
