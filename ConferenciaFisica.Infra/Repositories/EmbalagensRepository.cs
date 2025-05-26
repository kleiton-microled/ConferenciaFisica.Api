using Dapper;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Infra.Sql;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Infra.Repositories
{
    public class EmbalagensRepository : IEmbalagensRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public EmbalagensRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<TiposEmbalagens>> GetAll()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.CarregarTiposEmbalages;
                return await connection.QueryAsync<TiposEmbalagens>(sql);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
