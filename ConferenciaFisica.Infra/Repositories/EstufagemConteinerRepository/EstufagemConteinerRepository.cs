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

                //string ttlQuery = SqlQueries.BuscarValorTTL;
                //var ttl = await connection.QueryFirstOrDefaultAsync<int>(ttlQuery, new { planejamento }, transaction);
                //if (ttl > 0)
                //    ret.Ttl = ttl;

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

        public async Task<SaldoCargaMarcanteDto> BuscarSaldoCargaMarcante(int planejamento, string codigoMarcante)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;

            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                string query = SqlQueries.BuscarSaldoCargaMarcante;
                var ret = await connection.QueryFirstOrDefaultAsync<SaldoCargaMarcanteDto>(query, new { planejamento, codigoMarcante }, transaction);

                await transaction.CommitAsync();
                return ret;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar: {ex.Message}", ex);
            }
        }

        public async Task<bool> IniciarEstufagem(TalieInsertDTO talie)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = SqlQueries.IniciarEstufagem;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AutonumPatio", talie.AutonumPatio);
            parameters.Add("@AutonumBoo", talie.AutonumBoo);
            parameters.Add("@FormaOperacao", talie.Operacao);
            parameters.Add("@Conferente", talie.Conferente);
            parameters.Add("@Equipe", talie.Equipe);
            parameters.Add("@AutonumRo", talie.AutonumRo);


            var result = await connection.ExecuteAsync(query, parameters);

            return result > 0;
        }
    }
}
