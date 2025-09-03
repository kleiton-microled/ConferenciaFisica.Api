using ConferenciaFisica.Contracts.Common;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.PixMonitoring.Interfaces
{
    public interface IPixMonitoringUseCase
    {
        Task<ServiceResult<IEnumerable<PixPagamento>>> ListAllPixAsync();
        Task<ServiceResult<PaginationResult<PixPagamento>>> ListAllPixWithPaginationAsync(PaginationInput pagination);
        Task<ServiceResult<PaginationResult<PixPagamento>>> ListPixWithFilterAsync(PixFiltroInput filtro);
        Task<ServiceResult<int>> GetTotalPixAtivosAsync();
        Task<ServiceResult<int>> GetTotalPixPagosAsync();
        Task<ServiceResult<int>> GetTotalPixCanceladosAsync();
        Task<ServiceResult<int>> GetTotalPixAtivosComFiltroAsync(PixFiltroInput filtro);
        Task<ServiceResult<int>> GetTotalPixPagosComFiltroAsync(PixFiltroInput filtro);
        Task<ServiceResult<int>> GetTotalPixCanceladosComFiltroAsync(PixFiltroInput filtro);
    }
} 