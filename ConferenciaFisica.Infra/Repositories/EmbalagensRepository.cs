using Dapper;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Infra.Sql;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Application.Interfaces;
using System.Linq;

namespace ConferenciaFisica.Infra.Repositories
{
    public class EmbalagensRepository : IEmbalagensRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;
        private readonly ISchemaService _schemaService;

        public EmbalagensRepository(SqlServerConnectionFactory connectionFactory, ISchemaService schemaService)
        {
            _connectionFactory = connectionFactory;
            _schemaService = schemaService;
        }

        public async Task<IEnumerable<TiposEmbalagens>> GetAll()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlSchemaHelper.ReplaceSchema(SqlQueries.CarregarTiposEmbalages, _schemaService);
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
