namespace ConferenciaFisica.Contracts.DTOs
{
    public class EstufagemNotaFiscalDto
    {
        public long AutonumPatio { get; set; }
        public long AutonumNfi { get; set; }
        public int QtdeEstufada { get; set; }

        public static EstufagemNotaFiscalDto New(long autonumPatio, long autonumNotaFiscalItem, int quantidadeEstufada)
        {
            var dto = new EstufagemNotaFiscalDto();
            dto.AutonumPatio = autonumPatio;
            dto.AutonumNfi = autonumNotaFiscalItem;
            dto.QtdeEstufada = quantidadeEstufada;
            
            return dto;
            
        }
    }

}
