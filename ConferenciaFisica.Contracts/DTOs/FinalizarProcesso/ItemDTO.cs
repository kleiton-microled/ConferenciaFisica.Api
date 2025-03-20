namespace ConferenciaFisica.Contracts.DTOs.FinalizarProcesso
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }
        public int Embalagem { get; set; }
        public int Produto { get; set; }
        public string Marca { get; set; } = string.Empty;
        public int QuantidadeEstufagem { get; set; }
        public decimal Comprimento { get; set; }
        public decimal Largura { get; set; }
        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
        public string Yard { get; set; } = string.Empty;
        public int Armazem { get; set; }
        public string IMO { get; set; } = string.Empty;
        public string IMO2 { get; set; } = string.Empty;
        public string IMO3 { get; set; } = string.Empty;
        public string IMO4 { get; set; } = string.Empty;
        public string UNO { get; set; } = string.Empty;
        public string UNO2 { get; set; } = string.Empty;
        public string UNO3 { get; set; } = string.Empty;
        public string UNO4 { get; set; } = string.Empty;
        public int RegistroCargaSolta { get; set; }
        public string CodigoEan { get; set; } = string.Empty;
        public string FclLcl { get; set; } = string.Empty;
        public int NotaFiscal { get; set; }
        public int EmbalagemReserva { get; set; }
        public string CodigoProduto { get; set; } = string.Empty;
        public int IdBookingCarga { get; set; }
    }
}
