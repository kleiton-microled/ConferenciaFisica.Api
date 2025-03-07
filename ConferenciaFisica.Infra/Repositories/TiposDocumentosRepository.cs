using Dapper;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Infra.Sql;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Infra.Repositories
{
    public class TiposDocumentosRepository : ITiposDocumentosRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public TiposDocumentosRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<LoteAgendamentoDto>> CarregarLotesAgendamentoAsync(string filtro)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
                SELECT 
                    LOTE AS Display, 
                    LOTE AS Autonum
                FROM TB_AGENDAMENTO_POSICAO A
                INNER JOIN TB_AGENDA_POSICAO_MOTIVO B ON A.AUTONUM = B.AUTONUM_AGENDA_POSICAO
                WHERE CONVERT(VARCHAR, DT_PREVISTA, 103) = CONVERT(VARCHAR, GETDATE(), 103)
                AND ID_STATUS_AGENDAMENTO = 0 
                AND LOTE IS NOT NULL";

            if (!string.IsNullOrEmpty(filtro))
            {
                sql += " AND LOTE = @Filtro";
            }

            return await connection.QueryAsync<LoteAgendamentoDto>(sql, new { Filtro = filtro });
        }

        public async Task<IEnumerable<ConteinerAgendamentoDto>> CarregarCntrAgendamentoAsync(string filtro)
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

            return await connection.QueryAsync<ConteinerAgendamentoDto>(sql, new { Filtro = filtro });
        }

        public async Task<IEnumerable<TipoDocumentos>> GetAll()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var sql = SqlQueries.CarregarTiposDocumentos;
                return await connection.QueryAsync<TipoDocumentos>(sql);

            }
            catch (Exception ex)
            {

                throw;
            }
            throw new NotImplementedException();
        }
    }
}
