using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenciaFisica.Infra.Repositories
{
    public class ColetorRepository : IColetorRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public ColetorRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<ConferenteDTO>> ListarConferentes()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.ListarConferentes;
                return await connection.QueryAsync<ConferenteDTO>(sql);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<EquipeDTO>> ListarEquipes()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.ListarEquipes;
                return await connection.QueryAsync<EquipeDTO>(sql);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
