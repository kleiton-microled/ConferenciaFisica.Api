using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities.PreRegistro;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IAgendamentoRepository
    {
        Task<IEnumerable<LoteAgendamentoDto>> CarregarLotesAgendamentoAsync(string filtro, List<string> patiosPermitidos);
        Task<IEnumerable<ConteinerAgendamentoDto>> CarregarCntrAgendamentoAsync(string filtro, List<string> patiospermitidos);
        Task<DadosAgendamentoModel?> PendenciaDeSaidaEstacionamento(string placa, string placaCarreta);
        Task<DadosAgendamentoModel?> PendenciaDeSaidaPatio(string placa);
        Task<DadosAgendamentoModel?> GetDadosAgendamento(string sistema, string placa, string placaCarreta);
        Task<int> GetPendenciaEntrada(string? placa, string? placaCarreta);
    }
}
