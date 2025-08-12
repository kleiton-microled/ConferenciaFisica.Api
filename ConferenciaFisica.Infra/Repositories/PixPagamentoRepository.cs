using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using ConferenciaFisica.Application.Common.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using ConferenciaFisica.Contracts.Common;

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
    }
} 