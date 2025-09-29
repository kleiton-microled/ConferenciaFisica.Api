using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using ConferenciaFisica.Application.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.ComponentModel;

namespace ConferenciaFisica.Infra.Repositories
{
    public class ConferenciaRepository : IConferenciaRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;
        private readonly ISchemaService _schemaService;

        public ConferenciaRepository(SqlServerConnectionFactory connectionFactory, ISchemaService schemaService)
        {
            _connectionFactory = connectionFactory;
            _schemaService = schemaService;
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

        public async Task<Conferencia> BuscarPorConteinerRedexAsync(string idConteiner)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.BuscarConferenciaRedexPorIdContainer;

                var ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idConteiner });
                if (ret is null)
                {
                    ret = new Conferencia();
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
                string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.BUscarConferenciaPorLote, _schemaService);

                var ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idLote });
                if (ret is null)
                {
                    query = SqlSchemaHelper.ReplaceSchema(SqlQueries.BuscarLotePorAgendamento, _schemaService);
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
            string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.BuscarConferenciaPorReserva, _schemaService);

            return await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { LotePesquisa = idLote });
        }

        public async Task<bool> IniciarConferencia(ConferenciaFisicaCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlSchemaHelper.BuildInsertConferenciaFisicaQuery(_schemaService);

            var parameters = new DynamicParameters();
            parameters.Add("Cntr", command.Cntr);
            
            // Só adiciona o parâmetro BL se o schema NÃO for REDEX
            if (_schemaService.GetCurrentSchema() != "REDEX")
                parameters.Add("Bl", command.Bl);

            parameters.Add("CpfConferente", command.CpfConferente);
            parameters.Add("NomeConferente", command.NomeConferente);
            parameters.Add("telefoneConferente", command.TelefoneConferente);
            parameters.Add("CpfCliente", command.CpfCliente);
            parameters.Add("NomeCliente", command.NomeCliente);
            parameters.Add("QuantidadeDivergente", command.QuantidadeDivergente);
            parameters.Add("DivergenciaQualificacao", command.DivergenciaQualificacao);
            parameters.Add("ObservacaoDivergencias", command.ObservacaoDivergencias);
            parameters.Add("RetiradaAmostra", command.RetiradaAmostra);
            parameters.Add("ConferenciaRemota", command.ConferenciaRemota);
            parameters.Add("QuantidadeVolumesDivergentes", command.QuantidadeVolumesDivergentes);
            parameters.Add("QuantidadeRepresentantes", command.QuantidadeRepresentantes);
            parameters.Add("QuantidadeAjudantes", command.QuantidadeAjudantes);
            parameters.Add("QuantidadeOperadores", command.QuantidadeOperadores);
            parameters.Add("Movimentacao", command.Movimentacao);
            parameters.Add("Desunitizacao", command.Desunitizacao);
            parameters.Add("QuantidadeDocumentos", command.QuantidadeDocumentos);
            parameters.Add("autonumAgendaPosicao", command.AutonumAgendaPosicao);
            parameters.Add("tipoConferencia", 1);

            var ret = await connection.ExecuteAsync(query, parameters);
            return ret > 0;
        }

        public async Task<bool> IniciarConferencia(
            string? cntr, string? bl, string? cpfConferente, string? nomeConferente,
            string? telefoneConferente, string? cpfCliente, string? nomeCliente,
            int? quantidadeDivergente, bool divergenciaQualificacao, string? observacaoDivergencias,
            int? retiradaAmostra, bool? conferenciaRemota, int? quantidadeVolumesDivergentes,
            int? quantidadeRepresentantes, int? quantidadeAjudantes, int? quantidadeOperadores,
            int? movimentacao, int? desunitizacao, int? quantidadeDocumentos, int? autonumAgendaPosicao)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlSchemaHelper.BuildInsertConferenciaFisicaQuery(_schemaService);

            var parameters = new DynamicParameters();
            parameters.Add("Cntr", cntr);
            
            // Só adiciona o parâmetro BL se o schema NÃO for REDEX
            if (_schemaService.GetCurrentSchema() != "REDEX")
                parameters.Add("Bl", bl);

            parameters.Add("CpfConferente", cpfConferente);
            parameters.Add("NomeConferente", nomeConferente);
            parameters.Add("telefoneConferente", telefoneConferente);
            parameters.Add("CpfCliente", cpfCliente);
            parameters.Add("NomeCliente", nomeCliente);
            parameters.Add("QuantidadeDivergente", quantidadeDivergente);
            parameters.Add("DivergenciaQualificacao", divergenciaQualificacao);
            parameters.Add("ObservacaoDivergencias", observacaoDivergencias);
            parameters.Add("RetiradaAmostra", retiradaAmostra);
            parameters.Add("ConferenciaRemota", conferenciaRemota);
            parameters.Add("QuantidadeVolumesDivergentes", quantidadeVolumesDivergentes);
            parameters.Add("QuantidadeRepresentantes", quantidadeRepresentantes);
            parameters.Add("QuantidadeAjudantes", quantidadeAjudantes);
            parameters.Add("QuantidadeOperadores", quantidadeOperadores);
            parameters.Add("Movimentacao", movimentacao);
            parameters.Add("Desunitizacao", desunitizacao);
            parameters.Add("QuantidadeDocumentos", quantidadeDocumentos);
            parameters.Add("autonumAgendaPosicao", autonumAgendaPosicao);

            var ret = await connection.ExecuteAsync(query, parameters);
            return ret > 0;
        }

        public async Task<bool> AtualizarConferencia(ConferenciaFisicaCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlSchemaHelper.BuildUpdateConferenciaFisicaQuery(_schemaService);

            var parameters = new DynamicParameters();
            
            // Só adiciona o parâmetro BL se o schema NÃO for REDEX
            if (_schemaService.GetCurrentSchema() != "REDEX")
                parameters.Add("bl", command.Bl);

            parameters.Add("cpfConferente", command.CpfConferente);
            parameters.Add("nomeConferente", command.NomeConferente);
            parameters.Add("telefoneConferente", command.TelefoneConferente);
            parameters.Add("cpfCliente", command.CpfCliente);
            parameters.Add("nomeCliente", command.NomeCliente);
            parameters.Add("retiradaAmostra", command.RetiradaAmostra);
            parameters.Add("quantidadeDivergente", command.QuantidadeDivergente);
            parameters.Add("divergenciaQualificacao", command.DivergenciaQualificacao);
            parameters.Add("observacaoDivergencias", command.ObservacaoDivergencias);
            parameters.Add("conferenciaRemota", command.ConferenciaRemota);
            parameters.Add("embalagem", command.Embalagem);
            parameters.Add("quantidadeRepresentantes", command.QuantidadeRepresentantes);
            parameters.Add("quantidadeAjudantes", command.QuantidadeAjudantes);
            parameters.Add("quantidadeOperadores", command.QuantidadeOperadores);
            parameters.Add("movimentacao", command.Movimentacao);
            parameters.Add("desunitizacao", command.Desunitizacao);
            parameters.Add("porcentagemDesunitizacao", command.PorcentagemDesunitizacao);
            parameters.Add("quantidadeDocumentos", command.QuantidadeDocumentos);
            parameters.Add("quantidadeVolumesDivergentes", command.QuantidadeVolumesDivergentes);
            parameters.Add("tipo", command.Tipo);
            parameters.Add("ID", command.Id);

            var ret = await connection.ExecuteAsync(query, parameters);
            return ret > 0;
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

                string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.CarregarTiposLacres, _schemaService);

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
                    string updateQuery = $@"UPDATE {_schemaService.GetTableName("TB_EFETIVACAO_CONF_FISICA")} SET QTD_REPRESENTANTES = QTD_REPRESENTANTES -1 WHERE ID = @idConferencia";
                    await connection.ExecuteAsync(updateQuery, parameters, transaction);
                }

                if (tipo == "Ajudantes")
                {
                    string updateQuery = $@"UPDATE {_schemaService.GetTableName("TB_EFETIVACAO_CONF_FISICA")} SET QTD_AJUDANTES = QTD_AJUDANTES -1 WHERE ID = @idConferencia";
                    await connection.ExecuteAsync(updateQuery, parameters, transaction);
                }

                if (tipo == "Operador")
                {
                    string updateQuery = $@"UPDATE {_schemaService.GetTableName("TB_EFETIVACAO_CONF_FISICA")} SET QTD_OPERADORES = QTD_OPERADORES -1 WHERE ID = @idConferencia";
                    await connection.ExecuteAsync(updateQuery, parameters, transaction);
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

                string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.CarregarLacresConferencia, _schemaService);

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
            string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.CadastrarLacreConferencia, _schemaService);

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
            string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.AtualizarLacreConferencia, _schemaService);

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

                string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.ExcluirLacreConferencia, _schemaService);

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

                string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.CarregarDocumentosConferencia, _schemaService);

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
                string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.CadastrarDocumentosConferencia, _schemaService);
                var countDocumentos = await connection.ExecuteScalarAsync<int>(query, command, transaction);

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("qtdDocumentos", countDocumentos);
                parameters.Add("idConferenciaFisica", command.IdConferencia);
                if (countDocumentos > 0)
                {
                    //atualizar documentos na tb efetivacao
                    string update = $@"UPDATE {_schemaService.GetTableName("TB_EFETIVACAO_CONF_FISICA")} SET QTD_DOCUMENTOS = @qtdDocumentos WHERE ID = @idConferenciaFisica";
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
            string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.AtualizarDocumentosConferencia, _schemaService);

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
                string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.ExcluirDocumentosConferencia, _schemaService);
                var ret = await connection.ExecuteAsync(query, new { id }, transaction);


                if (ret > 0)
                {
                    DynamicParameters parameters = new DynamicParameters();

                    parameters.Add("idConferenciaFisica", idConferencia);

                    //atualizar documentos na tb efetivacao
                    string update = $@"UPDATE {_schemaService.GetTableName("TB_EFETIVACAO_CONF_FISICA")} SET QTD_DOCUMENTOS = QTD_DOCUMENTOS -1 WHERE ID = @idConferenciaFisica";
                    await connection.ExecuteAsync(update, parameters);

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

                string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.FinalizarConferencia, _schemaService);

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

                string query = SqlSchemaHelper.ReplaceSchema(
                    _schemaService.GetCurrentSchema() != "REDEX" ? SqlQueries.BuscarConferenciaPorId : SqlQueries.BuscarConferenciaRedexPorId, _schemaService);

                var ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { id });

                if (ret is null)
                {
                    query = SqlSchemaHelper.ReplaceSchema(SqlQueries.BuscarConferenciaPorId, _schemaService);
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
                string query = $@"SELECT AUTONUM FROM {_schemaService.GetTableName("TB_CNTR_BL")} tcb WHERE tcb.ID_CONTEINER = @idConteiner";

                return connection.QueryFirstOrDefault<int>(query, new { idConteiner = idConteiner });
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<string> BuscarCpfConferente(string conferente)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlSchemaHelper.ReplaceSchema(
                    _schemaService.GetCurrentSchema() != "REDEX" ? SqlQueries.BuscarCpfConferente : SqlQueries.BuscarCpfConferente, _schemaService);

                var ret = await connection.QueryFirstOrDefaultAsync<string>(query, new { conferente });

                if (ret is null)
                {
                    query = SqlSchemaHelper.ReplaceSchema(SqlQueries.BuscarConferenciaPorId, _schemaService);
                    ret = await connection.QueryFirstOrDefaultAsync<string>(query, new { conferente });
                }

                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
