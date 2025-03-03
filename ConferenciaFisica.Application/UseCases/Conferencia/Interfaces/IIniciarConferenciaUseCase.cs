using ConferenciaFisica.Application.Commands;
using ConferenciaFisica.Application.Common.Models;

namespace ConferenciaFisica.Application.UseCases.Conferencia
{
    public interface IIniciarConferenciaUseCase
    {
        Task<ServiceResult<bool>> ExecuteAsync(ConferenciaFisicaRequest request);
    }
}
