using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Entities.DescargaExportacao;
using ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using System.ComponentModel;

namespace ConferenciaFisica.Infra.Repositories.DescargaExportacaoRepository
{
    public class DescargaExportacaoRepository : IDescargaExportacaoRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;
        public DescargaExportacaoRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<DescargaExportacao> BuscarRegistroAsync(int registro)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.CarregarRegistro;

                var ret = await connection.QueryFirstOrDefaultAsync<DescargaExportacao>(query, new { registro });

                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
