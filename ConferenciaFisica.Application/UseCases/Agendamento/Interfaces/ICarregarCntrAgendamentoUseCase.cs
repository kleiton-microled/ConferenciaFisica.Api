using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Application.Common.Models;

namespace ConferenciaFisica.Application.UseCases.Agendamento.Interfaces
{
    public interface ICarregarCntrAgendamentoUseCase
    {
        Task<ServiceResult<IEnumerable<ConteinerAgendamentoDto>>> ExecuteAsync(string filtro);
    }
}
