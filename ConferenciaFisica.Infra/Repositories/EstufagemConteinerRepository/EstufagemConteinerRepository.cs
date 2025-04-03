using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories.EstufagemConteiner;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using Microsoft.Data.SqlClient;
using System.ComponentModel;

namespace ConferenciaFisica.Infra.Repositories.EstufagemConteinerRepository
{
    internal class EstufagemConteinerRepository : IEstufagemConteinerRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public EstufagemConteinerRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PlanejamentoDTO> BuscarPlanejamento(int planejamento)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;

            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                string query = SqlQueries.BuscarPlanejamento;
                var ret = await connection.QueryFirstOrDefaultAsync<PlanejamentoDTO>(query, new { planejamento }, transaction);

                string planQuery = SqlQueries.BUscarPlan;
                var plan = await connection.QueryFirstOrDefaultAsync<int>(planQuery, new { planejamento }, transaction);
                if(plan > 0)
                    ret.Plan = plan;

                await transaction.CommitAsync();
                return ret;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ItensEstufadosDTO>> BuscarItensEstufados(int patio)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;

            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                string query = SqlQueries.BuscarItensEstufados;
                var ret = await connection.QueryAsync<ItensEstufadosDTO>(query, new { patio }, transaction);

                await transaction.CommitAsync();
                return ret;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<EtiquetaDTO>> BuscarEtiquetas(int planejamento)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;

            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                string query = SqlQueries.BuscarEtiquetas;
                var ret = await connection.QueryAsync<EtiquetaDTO>(query, new { planejamento }, transaction);

                await transaction.CommitAsync();
                return ret;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar: {ex.Message}", ex);
            }
        }
    }
}
