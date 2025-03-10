using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Embalagens.Interfaces
{
    public interface IEmbalagensUseCase
    {
        Task<ServiceResult<IEnumerable<TiposEmbalagens>>> GetAllAsync();

    }
}
