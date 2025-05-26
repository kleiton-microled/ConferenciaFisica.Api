using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Equipes;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Conferentes
{
    public class EquipesUseCase : IEquipesUseCase
    {
        private readonly IColetorRepository _repository;

        public EquipesUseCase(IColetorRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<IEnumerable<EquipeDTO>>> ListarEquipes()
        {
            var data = await _repository.ListarEquipes();
            if (data == null)
            {
                return ServiceResult<IEnumerable<EquipeDTO>>.Success(data, "registros não encontrads.");
            }

            return ServiceResult<IEnumerable<EquipeDTO>>.Success(data, "Registros localizado com sucesso.");
        }
    }
}
