using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;

namespace ConferenciaFisica.Infra.Repositories
{
    public class TiposProcessoFotoRepository : ITiposProcessoFotoRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public TiposProcessoFotoRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<TipoProcessoFotoModel?> Create(TipoProcessoFotoModel input)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.InsertTiposProcessoFoto;

                var result = await connection.ExecuteAsync(query, input);

                return result > 0 ? input : null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.DeleteTipoProcessoFoto;

                var result = await connection.ExecuteAsync(query, new { id = id });

                return result > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TipoProcessoFotoModel?> Get(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.GetTiposProcessoFotoById;

                return await connection.QueryFirstOrDefaultAsync<TipoProcessoFotoModel>(sql, new { Id = id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TipoProcessoFotoModel>?> GetAll()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.ListarTiposProcessoFoto;

                return await connection.QueryAsync<TipoProcessoFotoModel>(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TipoProcessoFotoModel>> GetByProcessoNome(string processoName)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.GetTiposProcessoFotoByProcessoId;

                return await connection.QueryAsync<TipoProcessoFotoModel>(sql, new { ProcessDescription = processoName });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TipoProcessoFotoModel?> Update(TipoProcessoFotoModel input)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.UpdateTiposProcessoFoto;

                var result = await connection.ExecuteAsync(query, input);

                return result > 0 ? input : null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
