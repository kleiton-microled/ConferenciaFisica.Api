using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Application.UseCases.Patios
{
    public interface IPatiosUseCase
    {
        Task<ServiceResult<IEnumerable<PatiosDTO>>> ListarPatios();
    }
}
