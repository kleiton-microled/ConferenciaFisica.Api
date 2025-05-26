using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;

namespace ConferenciaFisica.Infra.Repositories
{
    public class TipoProcessoRepository : ITiposProcessoRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public TipoProcessoRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<TipoProcessoModel?> Create(TipoProcessoModel input)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.InsertTiposProcesso;

                var result = await connection.ExecuteAsync(query, input);

                return result > 0 ? input : null ;
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

                var query = SqlQueries.DeleteTipoProcesso;

                var result = await connection.ExecuteAsync(query, new { id = id });

                return result > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TipoProcessoModel?> Get(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.GetTiposProcessoById;

                return await connection.QueryFirstOrDefaultAsync<TipoProcessoModel>(sql, new {Id= id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TipoProcessoModel>?> GetAll()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.ListarTiposProcesso;

                return await connection.QueryAsync<TipoProcessoModel>(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TipoProcessoModel?> Update(TipoProcessoModel input)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.UpdateTiposProcesso;

                var result = await connection.ExecuteAsync(query, input);

                return result > 0 ? input :null;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
