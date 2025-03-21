using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.Agendamento.Interfaces;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.Agendamento
{
    public class CarregarLotesAgendamentoUseCase : ICarregarLotesAgendamentoUseCase
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public CarregarLotesAgendamentoUseCase(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<ServiceResult<IEnumerable<LoteAgendamentoDto>>> ExecuteAsync(string filtro)
        {
            var lotes = await _agendamentoRepository.CarregarLotesAgendamentoAsync(filtro);

            if (!lotes.Any())
                return ServiceResult<IEnumerable<LoteAgendamentoDto>>.Success(lotes, "Nenhum Lote não encontrado.");

            return ServiceResult<IEnumerable<LoteAgendamentoDto>>.Success(lotes, "Busca realizada com sucesso");
        }
    }
}
