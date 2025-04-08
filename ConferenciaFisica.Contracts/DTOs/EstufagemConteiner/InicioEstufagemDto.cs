namespace ConferenciaFisica.Contracts.DTOs.EstufagemConteiner
{
    public class TalieInsertDTO : TalieDTO
    {
        public long AutonumTalie { get; set; }
        public int AutonumPatio { get; set; }
        public int AutonumBoo { get; set; }
        public int AutonumRo { get; set; }
        public int AutonumCamera { get; set; }

        public bool FlagEstufagem { get; set; } = true;
        public bool FlagCarregamento { get; set; } = true;
        public bool Crossdocking { get; set; } = false;
    }

}
