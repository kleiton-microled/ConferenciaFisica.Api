using AutoMapper;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Marcantes
{
    public class MarcantesUseCase : IMarcantesUseCase
    {
        private readonly IMapper _mapper;
        private readonly IMarcantesRepository _repository;
        public MarcantesUseCase(IMapper mapper, IMarcantesRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<IEnumerable<MarcantesViewModel>> BuscarMarcantes(string pesquisa)
        {
            return _mapper.Map<IEnumerable<Marcante>, IEnumerable<MarcantesViewModel>>(await _repository.BuscarMarcantes(pesquisa));
        }
    }
}
