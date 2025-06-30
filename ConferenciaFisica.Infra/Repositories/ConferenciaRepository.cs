using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using Microsoft.Data.SqlClient;
using System.ComponentModel;

namespace ConferenciaFisica.Infra.Repositories
{
    public class ConferenciaRepository : IConferenciaRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public ConferenciaRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Conferencia> BuscarPorConteinerAsync(string idConteiner)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.BuscarConferenciaPorIdContainer;

                var ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idConteiner });

                if (ret is null)
                {
                    query = SqlQueries.BuscarConferenciaPorAgendamento;
                    ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idConteiner });
                }

                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<Conferencia> BuscarPorLoteAsync(string idLote)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                string query = SqlQueries.BUscarConferenciaPorLote;

                var ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idLote });
                if (ret is null)
                {
                    query = SqlQueries.BuscarLotePorAgendamento;
                    ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idLote });
                }

                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<Conferencia> BuscarPorReservaAsync(string idLote)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.BuscarConferenciaPorReserva;

            return await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { LotePesquisa = idLote });
        }

        public async Task<bool> IniciarConferencia(ConferenciaFisicaCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.InsertConferenciaFisica;

            var ret = await connection.ExecuteAsync(query, command);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AtualizarConferencia(ConferenciaFisicaCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.AtualizarConferencia;

            var ret = await connection.ExecuteAsync(query, command);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CadastroAdicional(CadastroAdicionalCommand command)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                string query = SqlQueries.CadastroAdicional;
                var ret = await connection.ExecuteAsync(query, command, transaction);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("tipo", command.Tipo);
                parameters.Add("idConferencia", command.IdConferencia);

                //buscar total cadastrado
                var countTipo = await connection.ExecuteScalarAsync<int>(@"SELECT COUNT(*) 
                                                                FROM TB_EFETIVACAO_CONF_FISICA_ADC 
                                                                WHERE ID_CONFERENCIA = @idConferencia
                                                                AND TIPO = @tipo", parameters, transaction);

                parameters.Add("countTipo", countTipo);

                if (command.Tipo == "Representantes")
                {
                    await connection.ExecuteAsync("UPDATE SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA SET QTD_REPRESENTANTES = @countTipo WHERE ID = @idConferencia", parameters, transaction);
                }

                if (command.Tipo == "Ajudantes")
                {
                    await connection.ExecuteAsync("UPDATE SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA SET QTD_AJUDANTES = @countTipo WHERE ID = @idConferencia", parameters, transaction);
                }

                if (command.Tipo == "Operador")
                {
                    await connection.ExecuteAsync("UPDATE SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA SET QTD_OPERADORES = @countTipo WHERE ID = @idConferencia", parameters, transaction);
                }

                await transaction.CommitAsync();
                return true;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar Cadastro: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<CadastrosAdicionaisDTO>> CarregarCadastrosAdicionais(int idConferencia)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.CarregarCadastrosAdicionais;

                var ret = await connection.QueryAsync<CadastrosAdicionaisDTO>(query, new { idConferencia });


                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<TipoLacre>> CarregarTiposLacres()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.CarregarTiposLacres;

                var ret = await connection.QueryAsync<TipoLacre>(query);


                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> Delete(int id, int? idConferencia, string? tipo)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                string query = SqlQueries.ExlcuirCadastroAdicional;
                var ret = await connection.ExecuteAsync(query, new { id }, transaction);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("idConferencia", idConferencia);

                if (tipo == "Representantes")
                {
                    await connection.ExecuteAsync("UPDATE SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA SET QTD_REPRESENTANTES = QTD_REPRESENTANTES -1 WHERE ID = @idConferencia", parameters, transaction);
                }

                if (tipo == "Ajudantes")
                {
                    await connection.ExecuteAsync("UPDATE SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA SET QTD_AJUDANTES = QTD_AJUDANTES -1 WHERE ID = @idConferencia", parameters, transaction);
                }

                if (tipo == "Operador")
                {
                    await connection.ExecuteAsync("UPDATE SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA SET QTD_OPERADORES = QTD_OPERADORES -1 WHERE ID = @idConferencia", parameters, transaction);
                }

                await transaction.CommitAsync();
                return true;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao deletar Cadastro: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Lacre>> CarregarLacresConferencia(int idConferencia)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.CarregarLacresConferencia;

                var ret = await connection.QueryAsync<Lacre>(query, new { idConferencia });


                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> CadastroLacreConferencia(LacreConferenciaCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.CadastrarLacreConferencia;

            var ret = await connection.ExecuteAsync(query, command);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AtualizarLacreConferencia(LacreConferenciaCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.AtualizarLacreConferencia;

            var ret = await connection.ExecuteAsync(query, command);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ExcluirLacreConferencia(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.ExcluirLacreConferencia;

                var ret = await connection.ExecuteAsync(query, new { id });
                if (ret > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<DocumentosConferencia>> CarregarDocumentosConferencia(int idConferencia)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.CarregarDocumentosConferencia;

                var ret = await connection.QueryAsync<DocumentosConferencia>(query, new { idConferencia });


                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> CadastroDocumentosConferencia(DocumentoConferenciaCommand command)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                string query = SqlQueries.CadastrarDocumentosConferencia;
                var countDocumentos = await connection.ExecuteScalarAsync<int>(query, command, transaction);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("qtdDocumentos", countDocumentos);
                parameters.Add("idConferenciaFisica", command.IdConferencia);
                if (countDocumentos > 0)
                {
                    //atualizar documentos na tb efetivacao
                    string update = @"UPDATE SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA SET QTD_DOCUMENTOS = @qtdDocumentos WHERE ID = @idConferenciaFisica";
                    await connection.ExecuteAsync(update, parameters, transaction);

                    await transaction.CommitAsync();
                }

                return true;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar Documentos: {ex.Message}", ex);
            }
        }

        public async Task<bool> AtualizarDocumentosConferencia(DocumentoConferenciaCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.AtualizarDocumentosConferencia;

            var ret = await connection.ExecuteAsync(query, command);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ExcluirDocumentosConferencia(int id, int? idConferencia)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                string query = SqlQueries.ExcluirDocumentosConferencia;
                var ret = await connection.ExecuteAsync(query, new { id }, transaction);


                if (ret > 0)
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("idConferenciaFisica", idConferencia);

                    //atualizar documentos na tb efetivacao
                    string update = @"UPDATE SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA SET QTD_DOCUMENTOS = QTD_DOCUMENTOS -1 WHERE ID = @idConferenciaFisica";
                    await connection.ExecuteAsync(update, parameters, transaction);

                    await transaction.CommitAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao excluir Documentos: {ex.Message}", ex);
            }
        }

        public async Task<bool> FinalizarConferencia(int idConferencia)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.FinalizarConferencia;

                var ret = await connection.ExecuteAsync(query, new { idConferencia });
                if (ret > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Conferencia> BuscarPorPorId(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.BuscarConferenciaPorId;

                var ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { id });

                if (ret is null)
                {
                    query = SqlQueries.BuscarConferenciaPorId;
                    ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { id });
                }

                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private int BuscarAutonumCntr(string idConteiner)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                string query = @"SELECT AUTONUM FROM SGIPA.dbo.TB_CNTR_BL tcb WHERE tcb.ID_CONTEINER = @idConteiner";

                return connection.QueryFirstOrDefault<int>(query, new { idConteiner = idConteiner });
            }
            catch (Exception ex)
            {

                throw;
            }


        }
    }
}
