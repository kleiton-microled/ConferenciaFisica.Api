using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Lacres.Interfaces;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Lacres
{
    public class TiposLacresUseCase : ITiposLacresUseCase
    {
        private readonly IConferenciaRepository _conferenciaRepository;

        public TiposLacresUseCase(IConferenciaRepository conferenciaRepository)
        {
            _conferenciaRepository = conferenciaRepository;
        }


        public async Task<ServiceResult<IEnumerable<TipoLacre>>> GetAllAsync()
        {
            var lacres = await _conferenciaRepository.CarregarTiposLacres();

            return ServiceResult<IEnumerable<TipoLacre>>.Success(lacres, "Lacres carregados com sucesso.");

        }

    }
}
