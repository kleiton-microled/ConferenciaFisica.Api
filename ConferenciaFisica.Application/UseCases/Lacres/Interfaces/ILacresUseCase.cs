using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Lacres.Interfaces
{
    public interface ILacresUseCase
    {
        Task<ServiceResult<IEnumerable<Lacre>>> GetAllAsync(int idConferencia);
        Task<ServiceResult<bool>> ExecuteAsync(LacreConferenciaInput request);
        Task<ServiceResult<bool>> UpdateAsync(LacreConferenciaInput request);
        Task<ServiceResult<bool>> DeleteAsync(int id);

    }
}
