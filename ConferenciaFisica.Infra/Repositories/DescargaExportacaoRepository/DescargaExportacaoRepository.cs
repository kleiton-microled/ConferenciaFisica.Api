using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Entities.DescargaExportacao;
using ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using System.ComponentModel;
using static Dapper.SqlMapper;

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
                string command = SqlQueries.CarregarRegistro;

                var registroDict = new Dictionary<int, DescargaExportacao>();
                var result = await connection.QueryAsync<DescargaExportacao, Talie, TalieItem, DescargaExportacao>(
                command,
                (registro, talie, talieItem) =>
                    {
                        if (!registroDict.TryGetValue(registro.Id, out var registroEntry))
                        {
                            registroEntry = registro;
                            registroEntry.Talie = talie;
                            if (registroEntry.Talie != null)
                                registroEntry.Talie.TalieItem = new List<TalieItem>();

                            registroDict.Add(registro.Id, registroEntry);
                        }

                        if (talieItem != null)
                        {
                            registroEntry.Talie.TalieItem.Add(talieItem);
                        }

                        return registroEntry;
                    },
                     new { registro },
                    splitOn: "Id,Id"
                );

                return registroDict.Values.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
