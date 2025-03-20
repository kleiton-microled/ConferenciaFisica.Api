namespace ConferenciaFisica.Contracts.DTOs.FinalizarProcesso
{
    public class FinalizarDescargaDTO
    {
        public int Id { get; set; }
        public DateTime? DataTermino { get; set; }
        public int IdBookingCarga { get; set; }
        public int Gate { get; set; }
        public string Operacao { get; set; } = string.Empty;

        public List<ItemDTO> Item { get; set; } = new List<ItemDTO>();
    }
}
