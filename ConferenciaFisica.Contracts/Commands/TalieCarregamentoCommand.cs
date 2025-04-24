namespace ConferenciaFisica.Contracts.Commands
{
    public class TalieCarregamentoInput
    {
        public long AutonumPatio { get; set; }
        public DateTime? DtInicioEstuf { get; set; }
        public DateTime? DtTerminoEstuf { get; set; }
        public long AutonumBoo { get; set; }
        public string? FormaOperacao { get; set; } // "M", "A", "P", etc.
        public long AutonumRomaneio { get; set; }
    }

}
