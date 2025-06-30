using Dapper;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Infra.Sql;
using ConferenciaFisica.Domain.Entities;
using System.Linq;

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
                var result = await connection.QueryAsync<TiposEmbalagens>(sql);

                var codigosPermitidos = new HashSet<int>
                {
                    917, 1009, 5, 10, 12, 13, 17, 19, 20, 23,
                    24, 26, 28, 32, 35, 36, 77, 938, 41, 75,
                    2, 76, 4, 42, 940, 44, 45, 49, 52, 56,
                    58, 59, 878, 921, 78, 98
                };

                return result.Where(x => codigosPermitidos.Contains(x.Id));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
