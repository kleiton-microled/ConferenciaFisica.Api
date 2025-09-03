using ConferenciaFisica.Contracts.Common;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Application.UseCases.PixMonitoring.Interfaces;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.PixMonitoring
{
    public class PixMonitoringUseCase : IPixMonitoringUseCase
    {
        private readonly IPixPagamentoRepository _pixPagamentoRepository;

        public PixMonitoringUseCase(IPixPagamentoRepository pixPagamentoRepository)
        {
            _pixPagamentoRepository = pixPagamentoRepository;
        }

        public async Task<ServiceResult<IEnumerable<PixPagamento>>> ListAllPixAsync()
        {
            try
            {
                var pixPagamentos = await _pixPagamentoRepository.ListarTodosAsync();
                
                if (pixPagamentos == null || !pixPagamentos.Any())
                {
                    return ServiceResult<IEnumerable<PixPagamento>>.Success(new List<PixPagamento>(), "Nenhum pagamento PIX encontrado.");
                }

                return ServiceResult<IEnumerable<PixPagamento>>.Success(pixPagamentos, "Pagamentos PIX listados com sucesso.");
            }
            catch (Exception ex)
            {
                return ServiceResult<IEnumerable<PixPagamento>>.Failure($"Erro ao listar pagamentos PIX: {ex.Message}");
            }
        }

        public async Task<ServiceResult<PaginationResult<PixPagamento>>> ListAllPixWithPaginationAsync(PaginationInput pagination)
        {
            try
            {
                var pixPagamentos = await _pixPagamentoRepository.ListarComPaginacaoAsync(pagination);
                
                if (pixPagamentos == null || !pixPagamentos.Data.Any())
                {
                    return ServiceResult<PaginationResult<PixPagamento>>.Success(pixPagamentos, "Nenhum pagamento PIX encontrado.");
                }

                return ServiceResult<PaginationResult<PixPagamento>>.Success(pixPagamentos, "Pagamentos PIX listados com sucesso.");
            }
            catch (Exception ex)
            {
                return ServiceResult<PaginationResult<PixPagamento>>.Failure($"Erro ao listar pagamentos PIX: {ex.Message}");
            }
        }

        public async Task<ServiceResult<int>> GetTotalPixAtivosAsync()
        {
            try
            {
                var totalPixAtivos = await _pixPagamentoRepository.GetTotalPixAtivosAsync();
                
                return ServiceResult<int>.Success(totalPixAtivos, $"Total de PIX ativos: {totalPixAtivos}");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Failure($"Erro ao obter total de PIX ativos: {ex.Message}");
            }
        }

        public async Task<ServiceResult<int>> GetTotalPixPagosAsync()
        {
            try
            {
                var totalPixPagos = await _pixPagamentoRepository.GetTotalPixPagosAsync();
                
                return ServiceResult<int>.Success(totalPixPagos, $"Total de PIX pagos: {totalPixPagos}");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Failure($"Erro ao obter total de PIX pagos: {ex.Message}");
            }
        }

        public async Task<ServiceResult<int>> GetTotalPixCanceladosAsync()
        {
            try
            {
                var totalPixCancelados = await _pixPagamentoRepository.GetTotalPixCanceladosAsync();
                
                return ServiceResult<int>.Success(totalPixCancelados, $"Total de PIX cancelados: {totalPixCancelados}");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Failure($"Erro ao obter total de PIX cancelados: {ex.Message}");
            }
        }

        public async Task<ServiceResult<PaginationResult<PixPagamento>>> ListPixWithFilterAsync(PixFiltroInput filtro)
        {
            try
            {
                var pixPagamentos = await _pixPagamentoRepository.ListarComFiltroAsync(filtro);
                
                if (pixPagamentos == null || !pixPagamentos.Data.Any())
                {
                    return ServiceResult<PaginationResult<PixPagamento>>.Success(pixPagamentos, "Nenhum pagamento PIX encontrado com os filtros aplicados.");
                }

                return ServiceResult<PaginationResult<PixPagamento>>.Success(pixPagamentos, "Pagamentos PIX filtrados com sucesso.");
            }
            catch (Exception ex)
            {
                return ServiceResult<PaginationResult<PixPagamento>>.Failure($"Erro ao listar pagamentos PIX com filtro: {ex.Message}");
            }
        }

        public async Task<ServiceResult<int>> GetTotalPixAtivosComFiltroAsync(PixFiltroInput filtro)
        {
            try
            {
                var totalPixAtivos = await _pixPagamentoRepository.GetTotalPixAtivosComFiltroAsync(filtro);
                
                return ServiceResult<int>.Success(totalPixAtivos, $"Total de PIX ativos com filtro: {totalPixAtivos}");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Failure($"Erro ao obter total de PIX ativos com filtro: {ex.Message}");
            }
        }

        public async Task<ServiceResult<int>> GetTotalPixPagosComFiltroAsync(PixFiltroInput filtro)
        {
            try
            {
                var totalPixPagos = await _pixPagamentoRepository.GetTotalPixPagosComFiltroAsync(filtro);
                
                return ServiceResult<int>.Success(totalPixPagos, $"Total de PIX pagos com filtro: {totalPixPagos}");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Failure($"Erro ao obter total de PIX pagos com filtro: {ex.Message}");
            }
        }

        public async Task<ServiceResult<int>> GetTotalPixCanceladosComFiltroAsync(PixFiltroInput filtro)
        {
            try
            {
                var totalPixCancelados = await _pixPagamentoRepository.GetTotalPixCanceladosComFiltroAsync(filtro);
                
                return ServiceResult<int>.Success(totalPixCancelados, $"Total de PIX cancelados com filtro: {totalPixCancelados}");
            }
            catch (Exception ex)
            {
                return ServiceResult<int>.Failure($"Erro ao obter total de PIX cancelados com filtro: {ex.Message}");
            }
        }
    }
} 