using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using ConferenciaFisica.Contracts.Common;
using System.Text;

namespace ConferenciaFisica.Infra.Repositories
{
    public class PixPagamentoRepository : IPixPagamentoRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public PixPagamentoRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<PixPagamento>> ListarTodosAsync()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.ListarTodosPix;

                var pixPagamentos = await connection.QueryAsync<PixPagamento>(query);

                return pixPagamentos;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar pagamentos PIX: {ex.Message}", ex);
            }
        }

        public async Task<PaginationResult<PixPagamento>> ListarComPaginacaoAsync(PaginationInput pagination)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                // Query para contar o total de registros
                string countQuery = SqlQueries.ContarTotalPix;
                var totalCount = await connection.ExecuteScalarAsync<int>(countQuery);

                // Query para buscar os dados com paginação
                string dataQuery = SqlQueries.ListarPixComPaginacao;
                var pixPagamentos = await connection.QueryAsync<PixPagamento>(
                    dataQuery,
                    new
                    {
                        Skip = pagination.Skip,
                        Take = pagination.Take
                    }
                );

                return new PaginationResult<PixPagamento>(
                    pixPagamentos,
                    totalCount,
                    pagination.PageNumber,
                    pagination.PageSize
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar pagamentos PIX com paginação: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalPixAtivosAsync()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.GetTotalPixAtivos;
                var totalCount = await connection.ExecuteScalarAsync<int>(query);

                return totalCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar PIX ativos: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalPixPagosAsync()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.GetTotalPixPagos;
                var totalCount = await connection.ExecuteScalarAsync<int>(query);

                return totalCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar PIX pagos: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalPixCanceladosAsync()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.GetTotalPixCancelados;
                var totalCount = await connection.ExecuteScalarAsync<int>(query);

                return totalCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar PIX cancelados: {ex.Message}", ex);
            }
        }

        public async Task<PaginationResult<PixPagamento>> ListarComFiltroAsync(PixFiltroInput filtro)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var (statusFilter, dateFilter, parameters) = BuildFilters(filtro);

                // Query para contar o total de registros com filtro
                string countQuery = string.Format(SqlQueries.ContarPixComFiltro, statusFilter, dateFilter);
                var totalCount = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

                // Query para buscar os dados com filtro e paginação
                string dataQuery = string.Format(SqlQueries.ListarPixComFiltro, statusFilter, dateFilter);
                
                // Adicionar parâmetros de paginação
                parameters.Add("Skip", filtro.Skip);
                parameters.Add("Take", filtro.Take);

                var pixPagamentos = await connection.QueryAsync<PixPagamento>(dataQuery, parameters);

                return new PaginationResult<PixPagamento>(
                    pixPagamentos,
                    totalCount,
                    filtro.PageNumber,
                    filtro.PageSize
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar pagamentos PIX com filtro: {ex.Message}", ex);
            }
        }

        private (string statusFilter, string dateFilter, DynamicParameters parameters) BuildFilters(PixFiltroInput filtro)
        {
            var statusFilter = new StringBuilder();
            var dateFilter = new StringBuilder();
            var parameters = new DynamicParameters();

            // Filtro de Status
            if (!string.IsNullOrEmpty(filtro.Status))
            {
                switch (filtro.Status.ToLower())
                {
                    case PixStatusFilter.Ativo:
                        statusFilter.Append(" AND (tpb.SEQ_GR = 0 OR tpb.SEQ_GR IS NULL) AND tpb.DATA_CADASTRO >= DATEADD(HOUR, -24, GETDATE())");
                        break;
                    case PixStatusFilter.Pago:
                        statusFilter.Append(" AND tpb.SEQ_GR > 0");
                        break;
                    case PixStatusFilter.Cancelado:
                        statusFilter.Append(" AND (tpb.SEQ_GR = 0 OR tpb.SEQ_GR IS NULL) AND tpb.DATA_CADASTRO < DATEADD(HOUR, -24, GETDATE())");
                        break;
                }
            }

            // Filtro de Data
            if (filtro.DataCriacaoInicial.HasValue || filtro.DataCriacaoFinal.HasValue)
            {
                if (filtro.DataCriacaoInicial.HasValue)
                {
                    dateFilter.Append(" AND tpb.DATA_CADASTRO >= @DataInicial");
                    parameters.Add("DataInicial", filtro.DataCriacaoInicial.Value);
                }

                if (filtro.DataCriacaoFinal.HasValue)
                {
                    dateFilter.Append(" AND tpb.DATA_CADASTRO <= @DataFinal");
                    parameters.Add("DataFinal", filtro.DataCriacaoFinal.Value.AddDays(1).AddTicks(-1)); // Incluir até o final do dia
                }
            }

            return (statusFilter.ToString(), dateFilter.ToString(), parameters);
        }

        public async Task<int> GetTotalPixAtivosComFiltroAsync(PixFiltroInput filtro)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var (dateFilter, parameters) = BuildDateFilter(filtro);

                string query = string.Format(SqlQueries.ContarPixAtivosComFiltro, dateFilter);
                var totalCount = await connection.ExecuteScalarAsync<int>(query, parameters);

                return totalCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar PIX ativos com filtro: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalPixPagosComFiltroAsync(PixFiltroInput filtro)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var (dateFilter, parameters) = BuildDateFilter(filtro);

                string query = string.Format(SqlQueries.ContarPixPagosComFiltro, dateFilter);
                var totalCount = await connection.ExecuteScalarAsync<int>(query, parameters);

                return totalCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar PIX pagos com filtro: {ex.Message}", ex);
            }
        }

        public async Task<int> GetTotalPixCanceladosComFiltroAsync(PixFiltroInput filtro)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var (dateFilter, parameters) = BuildDateFilter(filtro);

                string query = string.Format(SqlQueries.ContarPixCanceladosComFiltro, dateFilter);
                var totalCount = await connection.ExecuteScalarAsync<int>(query, parameters);

                return totalCount;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao contar PIX cancelados com filtro: {ex.Message}", ex);
            }
        }

        private (string dateFilter, DynamicParameters parameters) BuildDateFilter(PixFiltroInput filtro)
        {
            var dateFilter = new StringBuilder();
            var parameters = new DynamicParameters();

            // Filtro de Data
            if (filtro.DataCriacaoInicial.HasValue || filtro.DataCriacaoFinal.HasValue)
            {
                if (filtro.DataCriacaoInicial.HasValue)
                {
                    dateFilter.Append(" AND tpb.DATA_CADASTRO >= @DataInicial");
                    parameters.Add("DataInicial", filtro.DataCriacaoInicial.Value);
                }

                if (filtro.DataCriacaoFinal.HasValue)
                {
                    dateFilter.Append(" AND tpb.DATA_CADASTRO <= @DataFinal");
                    parameters.Add("DataFinal", filtro.DataCriacaoFinal.Value.AddDays(1).AddTicks(-1)); // Incluir até o final do dia
                }
            }

            return (dateFilter.ToString(), parameters);
        }
    }
} 