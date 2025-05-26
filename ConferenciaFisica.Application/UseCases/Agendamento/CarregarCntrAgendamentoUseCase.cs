using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Application.UseCases.Agendamento.Interfaces;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ConferenciaFisica.Application.UseCases.Agendamento
{
    public class CarregarCntrAgendamentoUseCase : ICarregarCntrAgendamentoUseCase
    {
        private readonly IPatioAccessService _patioAccessService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAgendamentoRepository _agendamentoRepository;

        public CarregarCntrAgendamentoUseCase(IPatioAccessService patioAccessService,
                                              IHttpContextAccessor httpContextAccessor,
                                              IAgendamentoRepository agendamentoRepository)
        {
            _patioAccessService = patioAccessService;
            _httpContextAccessor = httpContextAccessor;
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<ServiceResult<IEnumerable<ConteinerAgendamentoDto>>> ExecuteAsync(string filtro)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var patiosPermitidos = await _patioAccessService.GetPatiosPermitidos(user);

            if (patiosPermitidos == null || !patiosPermitidos.Any())
                return ServiceResult<IEnumerable<ConteinerAgendamentoDto>>.Failure("Usuário sem permissão de acesso a nenhum pátio.");


            var conteineres = await _agendamentoRepository.CarregarCntrAgendamentoAsync(filtro, patiosPermitidos);

            if (!conteineres.Any())
                return ServiceResult<IEnumerable<ConteinerAgendamentoDto>>.Success(conteineres, "Nenhum contêiner agendado para conferência.");

            return ServiceResult<IEnumerable<ConteinerAgendamentoDto>>.Success(conteineres);
        }
    }
}
