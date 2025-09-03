namespace ConferenciaFisica.Domain.Entities
{
    public class PixPagamento
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public DateTime DataCriacao { get; set; }
        public decimal Valor { get; set; }
        public bool Status { get; set; }
        public string StatusBaixa { get; set; }
        public string Validade { get; set; }
        public DateTime? DataPagamento { get; set; }
        public int BL { get; set; }
        public int GR { get; set; }
    }
} 