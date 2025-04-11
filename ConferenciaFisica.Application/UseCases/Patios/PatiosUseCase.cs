using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Patios
{
    public class PatiosUseCase : IPatiosUseCase
    {
        private readonly IColetorRepository _repository;

        public PatiosUseCase(IColetorRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<IEnumerable<PatiosDTO>>> ListarPatios()
        {
            var data = await _repository.ListarPatios();
            if (data == null)
            {
                return ServiceResult<IEnumerable<PatiosDTO>>.Success(data, "registros não encontrads.");
            }

            return ServiceResult<IEnumerable<PatiosDTO>>.Success(data, "Registros localizado com sucesso.");
        }
    }
}
