using Dapper;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Infra.Sql;

namespace ConferenciaFisica.Infra.Repositories
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public AgendamentoRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<LoteAgendamentoDto>> CarregarLotesAgendamentoAsync(string filtro, List<int> patiosPermitidos)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = SqlQueries.CarregarLotesAgendamentos;

            if (!string.IsNullOrEmpty(filtro))
            {
                sql += " AND LOTE = @Filtro";
            }

            return await connection.QueryAsync<LoteAgendamentoDto>(sql, new { Filtro = filtro, patiosPermitidos });
        }

        public async Task<IEnumerable<ConteinerAgendamentoDto>> CarregarCntrAgendamentoAsync(string filtro, List<int> patiospermitidos)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = SqlQueries.CarregarConteinerAgendamento;

            if (!string.IsNullOrEmpty(filtro))
            {
                sql += " AND ID_CONTEINER = @Filtro";
            }

            sql += SqlQueries.CarregarConteinerAgendamentoUnion;

            if (!string.IsNullOrEmpty(filtro))
            {
                sql += " AND ID_CONTEINER = @Filtro";
            }

            return await connection.QueryAsync<ConteinerAgendamentoDto>(sql, new { Filtro = filtro, patiospermitidos });
        }
    }
}
