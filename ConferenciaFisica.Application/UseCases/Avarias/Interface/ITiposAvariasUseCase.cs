using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Avarias.Interface
{
    public interface ITiposAvariasUseCase
    {
        Task<ServiceResult<IEnumerable<TiposAvarias>>> GetAllAsync();

    }
}
