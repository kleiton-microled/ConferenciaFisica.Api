using Dapper;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Infra.Sql;
using ConferenciaFisica.Domain.Entities.PreRegistro;
using ConferenciaFisica.Application.Interfaces;

namespace ConferenciaFisica.Infra.Repositories
{
    public class AgendamentoRepository : IAgendamentoRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;
        private readonly ISchemaService _schemaService;

        public AgendamentoRepository(SqlServerConnectionFactory connectionFactory, ISchemaService schemaService)
        {
            _connectionFactory = connectionFactory;
            _schemaService = schemaService;
        }

        public async Task<IEnumerable<LoteAgendamentoDto>> CarregarLotesAgendamentoAsync(string filtro, List<string> patiosPermitidos)
        {
            // Lotes sempre usam SGIPA, independente do ambiente
            using var connection = _connectionFactory.CreateLotesConnection();

            var sql = SqlQueries.CarregarLotesAgendamentos_v2;
            var patiosParam = filtro;

            return await connection.QueryAsync<LoteAgendamentoDto>(sql, new { patiosPermitidos = patiosParam });
        }

        public async Task<IEnumerable<ConteinerAgendamentoDto>> CarregarCntrAgendamentoAsync(string filtro, List<string> patiospermitidos)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = SqlQueries.CarregarConteinerAgendamento_v3;

            var patiosParam = filtro;
            return await connection.QueryAsync<ConteinerAgendamentoDto>(sql, new { Filtro = filtro, patiospermitidos = patiosParam });
        }

        public async Task<DadosAgendamentoModel?> PendenciaDeSaidaEstacionamento(string placa, string placaCarreta)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = SqlQueries.GetPendenciaSaidaRedex;

            if (!string.IsNullOrWhiteSpace(placa))
            {
                query += " AND PLACA_CAVALO = @placa";
            }

            if (!string.IsNullOrWhiteSpace(placa))
            {
                query += " AND PLACA_CARRETA = @placaCarreta";
            }

            return await connection.QueryFirstOrDefaultAsync<DadosAgendamentoModel?>(query, new { placa = placa, placaCarreta = placaCarreta });
        }

        public async Task<DadosAgendamentoModel?> PendenciaDeSaidaPatio(string placa)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = SqlQueries.GetPendenciaSaidaPatioRedex;

            return await connection.QueryFirstOrDefaultAsync<DadosAgendamentoModel?>(query, new { placa = placa });
        }

        public async Task<DadosAgendamentoModel?> GetDadosAgendamento(string sistema, string placa, string placaCarreta)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = SqlQueries.GetPendenciaSaidaRedex;

            if (!string.IsNullOrWhiteSpace(placa))
            {
                query += " AND PLACA_CAVALO = @placa";
            }

            if (!string.IsNullOrWhiteSpace(placaCarreta))
            {
                query += " AND PLACA_CARRETA = @placaCarreta";
            }

            return await connection.QueryFirstOrDefaultAsync<DadosAgendamentoModel?>(query, new { placa = placa, placaCarreta = placaCarreta });
        }

        public async Task<int> GetPendenciaEntrada(string? placa, string? placaCarreta)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = SqlQueries.GetPendenciaEntradaRedex;

            if (!string.IsNullOrWhiteSpace(placa))
            {
                query += " AND PLACA_CAVALO = @placa";
            }

            if (!string.IsNullOrWhiteSpace(placa))
            {
                query += " AND PLACA_CARRETA = @placaCarreta";
            }

            return await connection.QuerySingleAsync<int>(query, new { placa = placa});
        }
    }
}
