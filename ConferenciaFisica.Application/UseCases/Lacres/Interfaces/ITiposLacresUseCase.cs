using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Lacres.Interfaces
{
    public interface ITiposLacresUseCase
    {
        Task<ServiceResult<IEnumerable<TipoLacre>>> GetAllAsync();
    }
}
