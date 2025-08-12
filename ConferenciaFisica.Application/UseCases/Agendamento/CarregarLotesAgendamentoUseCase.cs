using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Interfaces;
using ConferenciaFisica.Application.UseCases.Agendamento.Interfaces;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace ConferenciaFisica.Application.UseCases.Agendamento
{
    public class CarregarLotesAgendamentoUseCase : ICarregarLotesAgendamentoUseCase
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IPatioAccessService _patioAccessService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CarregarLotesAgendamentoUseCase(IAgendamentoRepository agendamentoRepository,
                                               IPatioAccessService patioAccessService,
                                               IHttpContextAccessor httpContextAccessor)
        {
            _agendamentoRepository = agendamentoRepository;
            _patioAccessService = patioAccessService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResult<IEnumerable<LoteAgendamentoDto>>> ExecuteAsync(string filtro)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var patiosPermitidos = new List<string>(); //await _patioAccessService.GetPatiosPermitidos(user);

            //if (patiosPermitidos == null || !patiosPermitidos.Any())
            //    return ServiceResult<IEnumerable<LoteAgendamentoDto>>.Failure("Usuário sem permissão de acesso a nenhum pátio.");

            var lotes = await _agendamentoRepository.CarregarLotesAgendamentoAsync(filtro, patiosPermitidos);

            if (!lotes.Any())
                return ServiceResult<IEnumerable<LoteAgendamentoDto>>.Success(lotes, "Nenhum Lote não encontrado.");

            return ServiceResult<IEnumerable<LoteAgendamentoDto>>.Success(lotes, "Busca realizada com sucesso");
        }
    }
}
