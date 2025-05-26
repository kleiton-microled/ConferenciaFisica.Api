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

        public async Task<bool> Estufar(SaldoCargaMarcanteDto request)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var query = SqlQueries.EstufarCarga;
                var idCs = await ObterProximoIdAsync("seq_SAIDA_CARGA");

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AutonumSc", idCs);
                parameters.Add("@AutonumPatio", request.AutonumPatio);
                parameters.Add("@AutonumRo", request.AutonumRo);
                parameters.Add("@AutonumPcs", request.AutonumPcs);
                parameters.Add("@QtdeSaida", 2);
                parameters.Add("@AutonumEmb", request.AutonumEmb);
                parameters.Add("@PesoBruto", request.Bruto);
                parameters.Add("@Altura", request.Altura);
                parameters.Add("@Comprimento", request.Comprimento);
                parameters.Add("@Largura", request.Largura);
                parameters.Add("@Volume", request.Saldo);//TODO
                parameters.Add("@IdConteiner", request.Conteiner);
                parameters.Add("@Mercadoria", 1003);
                parameters.Add("@AutonumNf", request.AutonumNf);
                parameters.Add("@AutonumTalie", request.AutonumTalie);
                parameters.Add("@CodProduto", request.AutonumPro);
                parameters.Add("@AutonumRcs", request.AutonumRcs);
                parameters.Add("@QtdeEstufar", 2);
                parameters.Add("@QtdeEstufada", 2);
                parameters.Add("@AutonumMarcante", request.IdMarcante);

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

        private async Task<int> ObterProximoIdAsync(string databaseName)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            string sql = $@"
                INSERT INTO REDEX.dbo.{databaseName} (DATA)
                OUTPUT inserted.AUTONUM
                VALUES (GETDATE())";

            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
