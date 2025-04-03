namespace ConferenciaFisica.Contracts.DTOs.EstufagemConteiner
{
    public class EtiquetaDTO
    {
        public string Lote { get; set; } = string.Empty;
        public int? QtdeSaida { get; set; }
        public string Etiqueta { get; set; } = string.Empty;
        public string Reserva { get; set; } = string.Empty;
        public string DescricaoEmbalagem { get; set; } = string.Empty;
        public string? CodigoBarras { get; set; }
        public int? AutonumSc { get; set; }
        public int? AutonumRcs { get; set; }
        public string CodigoProduto { get; set; } = string.Empty;
        public string DescricaoProduto { get; set; } = string.Empty;
        public string NumeroNotaFiscal { get; set; } = string.Empty;
    }
}
