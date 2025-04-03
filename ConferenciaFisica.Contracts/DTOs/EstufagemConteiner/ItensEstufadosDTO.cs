namespace ConferenciaFisica.Contracts.DTOs.EstufagemConteiner
{
    public class ItensEstufadosDTO
    {
        public int Nr { get; set; }                        // ROW_NUMBER()
        public int QtdSaida { get; set; }                 // SC.QTDE_SAIDA
        public int AutonumSc { get; set; }                 // SC.AUTONUM_SC
        public string? DescricaoEmbalagem { get; set; }    // EMB.DESCRICAO_EMB
        public string? CodigoProduto { get; set; }         // PCS.CODPRODUTO
        public string? DescricaoProduto { get; set; }      // PRO.DESC_PRODUTO
        public string? NumeroNotaFiscal { get; set; }      // NF.NUM_NF
        public string? Reserva { get; set; }               // BOO.REFERENCE
        public int AutonumBoo { get; set; }                // BOO.AUTONUM_BOO
        public string? Lote { get; set; }                  // BOO.OS
        public int AutonumSdBoo { get; set; }              // NF.AUTONUM_SD_BOO
        public int AutonumRcs { get; set; }                // SC.AUTONUM_RCS
        public string? CodigoBarra { get; set; }           // SC.CODPRODUTO

    }
}
