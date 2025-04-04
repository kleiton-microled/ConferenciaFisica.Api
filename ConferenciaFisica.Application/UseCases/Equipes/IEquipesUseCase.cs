using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Application.UseCases.Equipes
{
    public interface IEquipesUseCase
    {
        Task<ServiceResult<IEnumerable<EquipeDTO>>> ListarEquipes();
    }
}
