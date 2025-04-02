using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Application.UseCases.MovimentacaoCargaSolta
{
    public interface IMovimentacaoCargaSoltaUseCase
    {
        Task<ServiceResult<MovimentacaoCargaDTO>> BuscarCargaParaMovimentar(int idMarcante);
        Task<ServiceResult<bool>> Movimentar(MovimentacaoCargaDTO carga);
    }
}
