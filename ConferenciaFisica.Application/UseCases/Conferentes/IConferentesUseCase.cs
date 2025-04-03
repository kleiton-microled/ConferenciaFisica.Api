using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Application.UseCases.Conferentes
{
    public interface IConferentesUseCase
    {
        Task<ServiceResult<IEnumerable<ConferenteDTO>>> ListarConferentes();
    }
}
