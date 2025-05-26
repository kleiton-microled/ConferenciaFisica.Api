using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Avarias.Interface;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Avarias
{
    public class TiposAvariasUseCase : ITiposAvariasUseCase
    {
        private readonly IAvariasRepository _repository;

        public TiposAvariasUseCase(IAvariasRepository repository)
        {
            _repository = repository;   
        }
        public async Task<ServiceResult<IEnumerable<TiposAvarias>>> GetAllAsync()
        {
            var data = await _repository.CarregarTiposAvarias();

            return ServiceResult<IEnumerable<TiposAvarias>>.Success(data, "Tipos Avarias carregados com sucesso.");
        }
    }
}
