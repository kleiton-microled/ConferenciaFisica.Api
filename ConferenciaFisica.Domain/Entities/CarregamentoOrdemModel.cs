namespace ConferenciaFisica.Domain.Entities
{
    public class CarregamentoOrdemModel
    {
        public string PlacaC { get; set; }
        public string PlacaCarreta { get; set; }
        public string Modelo { get; set; }
        public string OrdemCarreg { get; set; }
        public decimal Quantidade { get; set; }
        public int Autonumcs { get; set; }
        public string Lote { get; set; }
        public string Item { get; set; }
        public string Embalagem { get; set; }
        public decimal QtdeCarregada { get; set; }
    }
}
