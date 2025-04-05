namespace ConferenciaFisica.Contracts.DTOs.EstufagemConteiner
{
    public class SaldoCargaMarcanteDto
    {
        public int AutonumRcs { get; set; }
        public int AutonumPcs { get; set; }
        public int AutonumBoo { get; set; }
        public string ReservaCarga { get; set; } = string.Empty;
        public string NumNf { get; set; } = string.Empty;
        public int AutonumNf { get; set; }
        public int Qtde { get; set; }
        public int Saldo { get; set; }
        public string Marcante { get; set; } = string.Empty;
    }

}
