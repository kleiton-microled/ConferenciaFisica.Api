using ConferenciaFisica.Contracts.Common;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.PixMonitoring.Interfaces
{
    public interface IPixMonitoringUseCase
    {
        Task<ServiceResult<IEnumerable<PixPagamento>>> ListAllPixAsync();
        Task<ServiceResult<PaginationResult<PixPagamento>>> ListAllPixWithPaginationAsync(PaginationInput pagination);
        Task<ServiceResult<int>> GetTotalPixAtivosAsync();
        Task<ServiceResult<int>> GetTotalPixPagosAsync();
        Task<ServiceResult<int>> GetTotalPixCanceladosAsync();
    }
} 