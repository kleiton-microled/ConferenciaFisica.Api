using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;
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
                if (plan > 0)
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
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var query = SqlQueries.IniciarEstufagem;
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AutonumPatio", talie.AutonumPatio);
                parameters.Add("@AutonumBoo", talie.AutonumBoo);
                parameters.Add("@FormaOperacao", talie.Operacao);
                parameters.Add("@Conferente", talie.Conferente);
                parameters.Add("@Equipe", talie.Equipe);
                parameters.Add("@AutonumRo", talie.AutonumRo);
                parameters.Add("@AutonumTalie", talie.Id);


                var result = await connection.ExecuteAsync(query, parameters, transaction);

                //Atualizar TB_ROMANEIO
                var update_query = SqlQueries.UpdateTbRomaneio;
                await connection.ExecuteAsync(update_query, parameters, transaction);

                await transaction.CommitAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar: {ex.Message}", ex);
            }

            
        }

        public async Task<bool> Finalizar(TalieInsertDTO talie)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var query = SqlQueries.AtualizarEstufagemTbTalie;
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AutonumPatio", talie.AutonumPatio);
                parameters.Add("@AutonumRo", talie.AutonumRo);
              
                var result = await connection.ExecuteAsync(query, parameters, transaction);

                //Atualizar TB_ROMANEIO
                var update_query = SqlQueries.AtualizarEstufagemTbRomaneio;
                await connection.ExecuteAsync(update_query, parameters, transaction);

                await transaction.CommitAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar: {ex.Message}", ex);
            }
        }

        public async Task<bool> Estufar(TalieInsertDTO talie)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var query = SqlQueries.EstufarCarga;
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AutonumPatio", talie.AutonumPatio);
                parameters.Add("@AutonumRo", talie.AutonumRo);
                parameters.Add("@AutonumSc", talie.AutonumRo);
                parameters.Add("@AutonumPcs", talie.AutonumRo);
                parameters.Add("@QtdeSaida", talie.AutonumRo);
                parameters.Add("@AutonumEmb", talie.AutonumRo);
                parameters.Add("@PesoBruto", talie.AutonumRo);
                parameters.Add("@Altura", talie.AutonumRo);
                parameters.Add("@Comprimento", talie.AutonumRo);
                parameters.Add("@Largura", talie.AutonumRo);
                parameters.Add("@Volume", talie.AutonumRo);
                parameters.Add("@AutonumPatio", talie.AutonumRo);
                parameters.Add("@IdConteiner", talie.AutonumRo);
                parameters.Add("@Mercadoria", talie.AutonumRo);
                parameters.Add("@AutonumNf", talie.AutonumRo);
                parameters.Add("@AutonumTalie", talie.AutonumRo);
                parameters.Add("@CodProduto", talie.AutonumRo);
                parameters.Add("@AutonumRcs", talie.AutonumRo);
               
                var result = await connection.ExecuteAsync(query, parameters, transaction);

                //Atualizar TB_MARCANTE_RDX
                var update_query = SqlQueries.EstufarUpdateTbMarcante;
                await connection.ExecuteAsync(update_query, parameters, transaction);

                //Atualizar TB_AMR_NF_SAIDA
                var update_amr_nf_saida = SqlQueries.EstufarUpdateTbAmrNfSaida;
                await connection.ExecuteAsync(update_amr_nf_saida, parameters, transaction);

                //Atualizar TB_PATIO
                var update_patio = SqlQueries.EstufarUpdateTbPatio;
                await connection.ExecuteAsync(update_patio, parameters, transaction);

                await transaction.CommitAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar: {ex.Message}", ex);
            }
        }
    }
}
