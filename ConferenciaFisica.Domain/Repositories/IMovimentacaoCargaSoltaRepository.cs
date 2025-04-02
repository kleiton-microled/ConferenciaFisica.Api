using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IMovimentacaoCargaSoltaRepository
    {
        Task<MovimentacaoCargaDTO> BuscarCargaParaMovimentar(int idMarcante);
        Task<bool> Movimentar(MovimentacaoCargaDTO carga);
    }
}
