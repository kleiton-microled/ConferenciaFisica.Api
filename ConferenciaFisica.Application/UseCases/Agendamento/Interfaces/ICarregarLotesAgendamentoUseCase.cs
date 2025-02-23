using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Application.UseCases.Agendamento.Interfaces
{
    public interface ICarregarLotesAgendamentoUseCase
    {
        Task<ServiceResult<IEnumerable<LoteAgendamentoDto>>> ExecuteAsync(string filtro);
    }
}
