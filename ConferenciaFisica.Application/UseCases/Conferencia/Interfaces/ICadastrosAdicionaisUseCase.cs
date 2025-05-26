using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Application.UseCases.Conferencia
{
    public interface ICadastrosAdicionaisUseCase
    {
        Task<ServiceResult<bool>> ExecuteAsync(CadastroAdicionalInput request);
        Task<ServiceResult<IEnumerable<CadastrosAdicionaisDTO>>> GetAllAsync(int idConferencia);
        Task<ServiceResult<bool>> DeleteAsync(int id);
    }
}
