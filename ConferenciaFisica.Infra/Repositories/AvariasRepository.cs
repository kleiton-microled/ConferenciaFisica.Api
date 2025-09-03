using Dapper;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Infra.Sql;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Application.Interfaces;
using Microsoft.Data.SqlClient;

namespace ConferenciaFisica.Infra.Repositories
{
    public class AvariasRepository : IAvariasRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;
        private readonly ISchemaService _schemaService;

        public AvariasRepository(SqlServerConnectionFactory connectionFactory, ISchemaService schemaService)
        {
            _connectionFactory = connectionFactory;
            _schemaService = schemaService;
        }

        public async Task<AvariasConferencia> BuscarAvariasConferencia(int idConferencia)
        {
            try
            {
                using var connection = await _connectionFactory.CreateConnectionAsync();

                var sql = SqlSchemaHelper.ReplaceSchema(SqlQueries.BuscarAvariasConferencia, _schemaService);

                var avariaDictionary = new Dictionary<int, AvariasConferencia>();

                var result = await connection.QueryAsync<AvariasConferencia, TiposAvarias, AvariasConferencia>(
                    sql,
                    (avaria, tipoAvaria) =>
                    {
                        if (!avariaDictionary.TryGetValue(avaria.Id, out var avariaEntry))
                        {
                            avariaEntry = avaria;
                            avariaEntry.TiposAvarias = new List<TiposAvarias>();
                            avariaDictionary.Add(avariaEntry.Id, avariaEntry);
                        }

                        if (tipoAvaria != null)
                        {
                            avariaEntry.TiposAvarias.Add(tipoAvaria);
                        }

                        return avariaEntry;
                    },
                    new { idConferencia },
                    splitOn: "ID"
                );

                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar Avarias Conferencia: {ex.Message}", ex);
            }
        }


        public async Task<int> CadastrarAvariaConferencia(CadastroAvariaConferenciaCommand command)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                int avariaConferenciaId;
                string insertQuery = SqlSchemaHelper.ReplaceSchema(SqlQueries.CadastrarAvariaConferencia, _schemaService) + " SELECT SCOPE_IDENTITY();";

                // Captura o ID gerado após a inserção
                avariaConferenciaId = await connection.ExecuteScalarAsync<int>(insertQuery, command, transaction);

                if (avariaConferenciaId == 0)
                    throw new Exception("Falha ao inserir AvariaConferencia.");

                // Insere os novos TiposAvarias
                if (command.TiposAvarias != null && command.TiposAvarias.Count > 0)
                {
                    string insertTipoQuery = SqlSchemaHelper.ReplaceSchema(SqlQueries.CadastrarTiposAvariaConferencia, _schemaService);

                    foreach (var tipoAvaria in command.TiposAvarias)
                    {
                        await connection.ExecuteAsync(insertTipoQuery, new
                        {
                            IdAvariaConferencia = avariaConferenciaId,
                            IdTipoAvaria = tipoAvaria.Id
                        }, transaction);
                    }
                }

                await transaction.CommitAsync();
                return avariaConferenciaId;
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

        public async Task<int> AtualizarAvariaConferencia(CadastroAvariaConferenciaCommand command)
        {
            await using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                string query = SqlSchemaHelper.ReplaceSchema(SqlQueries.AtualizarAvariaConferencia, _schemaService);

                // Criamos um objeto anônimo com apenas os campos necessários (sem a lista `TiposAvarias`)
                var parametros = new
                {
                    command.Id,
                    command.IdConferencia,
                    command.QuantidadeAvariada,
                    command.PesoAvariado,
                    command.IdEmbalagem,
                    command.Conteiner,
                    command.Observacao
                };

                // Atualiza a AvariaConferencia (agora sem erro)
                var ret = await connection.ExecuteAsync(query, parametros, transaction);
                if (ret == 0)
                    throw new Exception("Falha ao atualizar AvariaConferencia.");

                // Exclui os tipos de avaria associados antes de inserir os novos
                string queryDelete = SqlSchemaHelper.ReplaceSchema(SqlQueries.ExcluirTiposAvariasConferencia, _schemaService);
                await connection.ExecuteAsync(queryDelete, new { IdAvariaConferencia = command.Id }, transaction);

                // Insere os novos TiposAvarias
                if (command.TiposAvarias != null && command.TiposAvarias.Count > 0)
                {
                    string insertTipoQuery = SqlSchemaHelper.ReplaceSchema(SqlQueries.CadastrarTiposAvariaConferencia, _schemaService);

                    foreach (var tipoAvaria in command.TiposAvarias)
                    {
                        await connection.ExecuteAsync(insertTipoQuery, new
                        {
                            IdAvariaConferencia = command.Id,
                            IdTipoAvaria = tipoAvaria.Id
                        }, transaction);
                    }
                }

                await transaction.CommitAsync();
                return command.Id;
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



        public async Task<IEnumerable<TiposAvarias>> CarregarTiposAvarias()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlSchemaHelper.ReplaceSchema(SqlQueries.CarregarTiposAvarias, _schemaService);
                return await connection.QueryAsync<TiposAvarias>(sql);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
