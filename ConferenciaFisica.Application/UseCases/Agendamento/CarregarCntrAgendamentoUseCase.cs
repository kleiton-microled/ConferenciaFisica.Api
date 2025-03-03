using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Application.UseCases.Agendamento.Interfaces;

namespace ConferenciaFisica.Application.UseCases.Agendamento
{
    public class CarregarCntrAgendamentoUseCase : ICarregarCntrAgendamentoUseCase
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public CarregarCntrAgendamentoUseCase(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<ServiceResult<IEnumerable<ConteinerAgendamentoDto>>> ExecuteAsync(string filtro)
        {
            var conteineres = await _agendamentoRepository.CarregarCntrAgendamentoAsync(filtro);

            if (!conteineres.Any())
                return new ServiceResult<IEnumerable<ConteinerAgendamentoDto>> { Status = false, Mensagens = new List<string> { "Nenhum contêiner agendado para conferência." } };

            return new ServiceResult<IEnumerable<ConteinerAgendamentoDto>> { Result = conteineres };
        }
    }
}
