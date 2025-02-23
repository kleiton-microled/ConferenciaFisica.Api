using System.Data;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;

namespace ConferenciaFisica.Infra.Repositories
{
    public class ConferenciaRepository : IConferenciaRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public ConferenciaRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Conferencia> BuscarPorConteinerAsync(string idConteiner)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.BuscarConferencia;

                return await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idConteiner });
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<Conferencia> BuscarPorLoteAsync(string idLote)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string query = @"
                SELECT AUTONUM AS Id, BL AS NumeroBl, CNTR AS NumeroCntr, 
                       INICIO, TERMINO, NOME_CLIENTE, CPF_CLIENTE, QTDE_AVARIADA, OBS_AVARIA,
                       DIVERGENCIA_QTDE AS DivergenciaQuantidade, DIVERGENCIA_QUALIFICACAO, OBS_DIVERGENCIA, 
                       RETIRADA_AMOSTRA, 
                       CASE 
                         WHEN NVL(BL, 0) <> 0 THEN 'CARGA SOLTA'
                         WHEN NVL(CNTR, 0) <> 0 THEN 'CONTEINER'
                         ELSE 'REDEX'
                       END AS TipoCarga
                FROM SGIPA.TB_EFETIVACAO_CONF_FISICA
                WHERE BL = :idLote
            ";

            return await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idLote });
        }
    }
}
