using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IAgendamentoRepository
    {
        Task<IEnumerable<LoteAgendamentoDto>> CarregarLotesAgendamentoAsync(string filtro);
        Task<IEnumerable<ConteinerAgendamentoDto>> CarregarCntrAgendamentoAsync(string filtro);

    }
}
