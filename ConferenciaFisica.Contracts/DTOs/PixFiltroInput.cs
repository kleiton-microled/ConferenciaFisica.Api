using ConferenciaFisica.Contracts.Common;

namespace ConferenciaFisica.Contracts.DTOs
{
    public class PixFiltroInput : PaginationInput
    {
        /// <summary>
        /// Filtro por status do PIX (ativo, pago, cancelado)
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Data de criação inicial para filtro
        /// </summary>
        public DateTime? DataCriacaoInicial { get; set; }

        /// <summary>
        /// Data de criação final para filtro
        /// </summary>
        public DateTime? DataCriacaoFinal { get; set; }
    }

    public static class PixStatusFilter
    {
        public const string Ativo = "ativo";
        public const string Pago = "pago";
        public const string Cancelado = "cancelado";
    }
}
