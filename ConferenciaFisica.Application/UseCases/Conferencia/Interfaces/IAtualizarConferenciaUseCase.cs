using ConferenciaFisica.Application.Commands;
using ConferenciaFisica.Application.Common.Models;

namespace ConferenciaFisica.Application.UseCases.Conferencia
{
    public interface IAtualizarConferenciaUseCase
    {
        Task<ServiceResult<bool>> ExecuteAsync(ConferenciaFisicaRequest request);
    }
}
