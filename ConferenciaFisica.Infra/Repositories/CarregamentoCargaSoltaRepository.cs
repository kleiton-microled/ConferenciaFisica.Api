using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Entities.CarregamentoDeCargaSolta;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ConferenciaFisica.Infra.Repositories
{
    public class CarregamentoCargaSoltaRepository : ICarregamentoCargaSoltaRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;
        public CarregamentoCargaSoltaRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int?> GetAutonumCs(int aUTONUMCS, string placa)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                string query = SqlQueries.getCarregamento;

                var result = await connection.QueryFirstOrDefaultAsync<int?>(query, new { autonumCargaSolta = aUTONUMCS, placa = placa });

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public async Task<IEnumerable<ItemCarregadoModel>> GetItensCarregados(string placa, string tipo = "I")
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                string query = tipo == "I" ? SqlQueries.GetItensCarregadosTipoI : SqlQueries.GetItensCarregados;

                var result = await connection.QueryAsync<ItemCarregadoModel>(query, new { placa = placa });

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new List<ItemCarregadoModel>();
            }
        }

        public async Task<MarcantePatioModel> GetMarcanteByIdEPatio(int marcante, int? patio)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                string query = SqlQueries.GeMarcantePatio;

                var result = await connection.QueryFirstOrDefaultAsync<MarcantePatioModel>(query, new { marcanteId = marcante });

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public async Task<IEnumerable<CarregamentoOrdemModel>> GetOrdens(string placa)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                string query = SqlQueries.GetOrdensCarregamento;

                var result = await connection.QueryAsync<CarregamentoOrdemModel>(query, new { placa = placa });

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return new List<CarregamentoOrdemModel>();
            }
        }

        public async Task<QuantidadeCarregamentoModel?> GetQuantidadeCarregamento(int aUTONUMCS, string placa)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                string query = SqlQueries.getCarregamentoQuantidade;

                var result = await connection.QueryFirstOrDefaultAsync<QuantidadeCarregamentoModel?>(query, new { autonum = placa, placa = aUTONUMCS });

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }


        public async Task<int> ObterProximoIdAsync(string databaseName)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            string sql = $@"
                INSERT INTO {databaseName} (DATA)
                OUTPUT inserted.AUTONUM
                VALUES (GETDATE())";

            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<string[]?> GetVeiculosByPatio(int patio)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.GetVeiculos;

            var result = await connection.QueryAsync<string>(query, new { patioId = patio });

            return result.ToArray();
        }

        public async Task UpdateMarcanteAndCargaSolta(int marcante, int? patio, string local, string placa, MarcantePatioModel marcanteByQuery, int? itensCarregados)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                //int updateId;
                //string updateQuery = "UPDATE REDEX.dbo.TB_CARGA_SOLTA_YARD SET QUANTIDADE=0 WHERE AUTONUM=" + marcanteByQuery.AUTONUM_CS_YARD;

                //// Captura o ID gerado após a inserção
                //updateId = await connection.ExecuteScalarAsync<int>(updateQuery, transaction);

                //if (updateId == 0)
                //    throw new Exception("Falha ao atualizar local.");

                //var nextIdYard = await ObterProximoIdAsync("REDEX.dbo.SEQ_CARGA_SOLTA_YARD");
                //string insertTipoQuery = SqlQueries.InsertCargaSoltaYard;

                //await connection.ExecuteAsync(insertTipoQuery, new
                //{
                //    nextIdYard = nextIdYard,
                //    autonumCs = marcanteByQuery.AUTONUMCS,
                //    armazem = marcanteByQuery.AUTONUM_ARMAZEM,
                //    qtd = itensCarregados
                //});

                //var updateQury = @"UPDATE SGIPA.TB_MARCANTES 
                //                    SET 
                //                        AUTONUM_CS_YARD = @autonumCsYard,               -- Substitua pelo valor de AutonumCSYard
                //                        PLACA_C = @placa                    -- Substitua pela placa do veículo
                //                    WHERE 
                //                        AUTONUM = @numQuery;";

                //await connection.ExecuteAsync(updateQury, new
                //{
                //    autonumCsYard = nextIdYard,
                //    placa = placa,
                //    numQuery = marcante
                //}, transaction);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar AvariaConferencia: {ex.Message}", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }
        }

        public async Task<IEnumerable<int>?> GetCarregamentoId(string placa)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                string query = SqlQueries.GetCargaPorPlaca;

                var result = await connection.QueryAsync<int>(query, new { placa = placa });

                return result;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }

        }

        public async Task<bool> FinalizarRedexCargaSolta(string placa, DateTime? inicio, int conferenteEquipeId, int equipe)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var carregamentoQuery = SqlQueries.GetCargaPorPlaca;

                var itensByQuery = await connection.QueryAsync<int>(carregamentoQuery, new { placa = placa }, transaction: transaction);
                if (itensByQuery == null || !itensByQuery.Any()) return false;

                var retiradasMinutasCountQuery = SqlQueries.GetMinutasCount;
                foreach (var item in itensByQuery)
                {
                    var count = await connection.ExecuteScalarAsync<int>(retiradasMinutasCountQuery, new { itemId = item }, transaction);

                    if (count == 0) throw new Exception("Não consta emissao de minuta de retira desta carga - Saida não permitida");
                }

                var getRomaneioAndTalieQuery = SqlQueries.GetRomaneioAndTalieQuery;
                foreach (var item in itensByQuery)
                {
                    var resultRomaneioTalie = await connection.QueryAsync<RomaneioTalieModel>(getRomaneioAndTalieQuery, new { itemId = item }, transaction);

                    if (resultRomaneioTalie == null || !resultRomaneioTalie.Any()) throw new Exception("Romaneio Talie nao encontrado");

                    foreach (var talieRomaneio in resultRomaneioTalie)
                    {
                        var autoNumActual = talieRomaneio.AutonumTalie;
                        if (autoNumActual == 0)
                        {
                            var nextIdRegistro = await ObterProximoIdAsync("redex.dbo.seq_tb_talie");
                            var insertQuery = SqlQueries.InsertTalie;


                            var insertedId = await connection.ExecuteScalarAsync<int>(insertQuery, new
                            {
                                //autonum_talie = nextIdRegistro,
                                inicio_descarga = inicio,
                                boo_ro = talieRomaneio.BooRo,
                                conferente = conferenteEquipeId,
                                equipe = equipe,
                                autonum_ro = talieRomaneio.RomaneioId,
                                autonum_gate = talieRomaneio.GateSaidaID,
                                placa = placa

                            }, transaction);

                            autoNumActual = insertedId;
                        }
                        else
                        {
                            var updateQuery = @"UPDATE REDEX.dbo.TB_TALIE 
                                                SET FLAG_FECHADO = 1 
                                                WHERE autonum_ro = @itemId";

                            await connection.ExecuteAsync(updateQuery, new
                            {
                                itemId = autoNumActual

                            }, transaction);
                        }

                        var updateQueryRomaneio = @"UPDATE REDEX.dbo.TB_romaneio 
                                                    SET 
                                                        FLAG_historico = 1,
                                                        autonum_talie = @talieId
                                                    WHERE 
                                                        autonum_ro = @itemId";

                        await connection.ExecuteAsync(updateQueryRomaneio, new
                        {
                            talieId = autoNumActual,
                            itemId = item

                        }, transaction);

                        var updateQuerySaida = @"UPDATE REDEX.dbo.TB_SAIDA_CARGA 
                                                 SET 
                                                    FLAG_saida = 1,
                                                    autonum_talie = @talieId
                                                  WHERE 
                                                    autonum_ro = @itemId";

                        await connection.ExecuteAsync(updateQuerySaida, new
                        {
                            talieId = autoNumActual,
                            itemId = item

                        }, transaction);
                    }
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar AvariaConferencia: {ex.Message}", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }

            return true;
        }

        public async Task<DateTime?> IniciarCarregamento(string veiculo)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();
            DateTime? resultDate = null;
            try
            {
                var inputDate = DateTime.Now;
                var carregamentoQuery = @"UPDATE redex.dbo.tb_romaneio 
                                        SET inicio_coletor = @inicio
                                        WHERE autonum_ro IN (
                                            SELECT DISTINCT item 
                                            FROM redex.dbo.VW_COL_CAM_CARREGAMENTO 
                                            WHERE PLACA_C = @placa
                                        )";

                var resultQuery = await connection.ExecuteAsync(carregamentoQuery, new
                {
                    placa = veiculo,
                    inicio = inputDate

                }, transaction);

                resultDate = resultQuery > 0 ? inputDate : null;

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar AvariaConferencia: {ex.Message}", ex);
            }
            finally
            {
                await connection.CloseAsync();
            }

            return resultDate;
        }
    }
}
