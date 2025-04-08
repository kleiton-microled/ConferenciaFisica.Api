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
        public long AutonumPatio { get; set; }
        public long AutonumEmb { get; set; }
        public decimal Bruto { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Largura { get; set; }
        public decimal Altura { get; set; }
        public long AutonumPro { get; set; }
    }

}
