using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Embalagens.Interfaces;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Embalagens
{
    public class EmbalagensUseCase : IEmbalagensUseCase
    {
        private readonly IEmbalagensRepository _repository;
        public EmbalagensUseCase(IEmbalagensRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServiceResult<IEnumerable<TiposEmbalagens>>> GetAllAsync()
        {
            var data = await _repository.GetAll();

            return ServiceResult<IEnumerable<TiposEmbalagens>>.Success(data, "Embalagens carregadas com sucesso.");
        }
    }
}
