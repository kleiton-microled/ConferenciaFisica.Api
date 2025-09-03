using ConferenciaFisica.Contracts.Common;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IPixPagamentoRepository
    {
        Task<IEnumerable<PixPagamento>> ListarTodosAsync();
        Task<PaginationResult<PixPagamento>> ListarComPaginacaoAsync(PaginationInput pagination);
        Task<PaginationResult<PixPagamento>> ListarComFiltroAsync(PixFiltroInput filtro);
        Task<int> GetTotalPixAtivosAsync();
        Task<int> GetTotalPixPagosAsync();
        Task<int> GetTotalPixCanceladosAsync();
        Task<int> GetTotalPixAtivosComFiltroAsync(PixFiltroInput filtro);
        Task<int> GetTotalPixPagosComFiltroAsync(PixFiltroInput filtro);
        Task<int> GetTotalPixCanceladosComFiltroAsync(PixFiltroInput filtro);
    }
} 