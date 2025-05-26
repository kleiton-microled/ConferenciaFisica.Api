using Dapper;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Sql;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Infra.Repositories
{
    public class MarcanteRepository : IMarcantesRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public MarcanteRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<MovimentacaoCargaDTO>> BuscarCargaParaMovimentar(int idMarcante)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.BuscarCargaParaMovimentar;

            var ret = await connection.QueryAsync<MovimentacaoCargaDTO>(query, idMarcante);

            return ret;
        }

        public async Task<IEnumerable<Marcante>> BuscarMarcantes(string pesquisa)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.BuscarMarcantesPorTermo;

            var ret = await connection.QueryAsync<Marcante>(query, new { term = $"%{pesquisa}%" });

            return ret;
        }
    }
}
