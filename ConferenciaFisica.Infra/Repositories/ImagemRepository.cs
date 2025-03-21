using System.Transactions;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using Microsoft.Win32;

namespace ConferenciaFisica.Infra.Repositories
{
    public class ImagemRepository : IImagemRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public ImagemRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> CreateProcesso(ProcessoCommand input)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.InsertProcesso;

                var result = await connection.ExecuteAsync(query, input);

                return result > 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> CreateTipoFoto(TipoFotoModel input)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.InsertTipoProcesso;

                var result = await connection.ExecuteAsync(query, input);

                return result > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteProcesso(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.DeleteProcesso;

                var result = await connection.ExecuteAsync(query, new { id = id });

                return result > 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> DeleteTipoFoto(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.DeleteTipoFoto;

                var result = await connection.ExecuteAsync(query, new { id = id });

                return result > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TipoFotoModel>> GetImagesTypes()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.ListarTiposFoto;
                return await connection.QueryAsync<TipoFotoModel>(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Processo>> ListProcessoByTalieId(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.ListarProcessosPorTalie;

                return await connection.QueryAsync<Processo>(sql, new { talieId = id });

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateProcesso(ProcessoCommand input)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.UpdatesProcessoDescricaoAndObservacao;

                var result = await connection.ExecuteAsync(sql, input);

                return result > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateTipoFoto(TipoFotoModel input)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.UpdatesTipoFoto;

                var result = await connection.ExecuteAsync(sql, input);

                return result > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
