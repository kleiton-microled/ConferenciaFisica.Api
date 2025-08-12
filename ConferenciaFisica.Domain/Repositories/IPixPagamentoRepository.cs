using ConferenciaFisica.Contracts.Common;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IPixPagamentoRepository
    {
        Task<IEnumerable<PixPagamento>> ListarTodosAsync();
        Task<PaginationResult<PixPagamento>> ListarComPaginacaoAsync(PaginationInput pagination);
        Task<int> GetTotalPixAtivosAsync();
        Task<int> GetTotalPixPagosAsync();
        Task<int> GetTotalPixCanceladosAsync();
    }
} 