using System.Net.Sockets;
using System.Numerics;
using ConferenciaFisica.Domain.Entities.PreRegistro;
using ConferenciaFisica.Domain.Enums;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;

namespace ConferenciaFisica.Infra.Repositories
{
    public class PreRegistroRepository : IPreRegistroRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public PreRegistroRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<bool> AtualizarDataChegada(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = @"UPDATE REDEX..TB_PRE_REGISTRO SET DATA_CHEGADA = GetDate(), Local = 1, FLAG_DEIC_PATIO = 0 WHERE AUTONUM = @Id";

            var result =  await connection.ExecuteAsync(query, new { Id = id });

            return result > 0;
        }

        public async Task<bool> Cadastrar(string? protocolo, string? placa, string? placaCarreta, string? ticket, LocalPatio localPatio, DateTime now1, DateTime? now2, bool v, string? finalidadeId, int? patioDestinoId)
        {

            using var connection = _connectionFactory.CreateConnection();
            var query = @"
                    SELECT 
	                    TOP 1 AUTONUM  AS AgendamentoId 
                    FROM 
	                    REDEX..TB_PRE_REGISTRO 
                    WHERE 
	                    PLACA = @Placa
                    AND 
	                    (DATA_CHEGADA IS NULL   AND SAIDA_DEIC_PATIO IS NOT  NULL AND DATA_CHEGADA_DEIC_PATIO IS NOT NULL) 
                    ORDER BY 
	                    AUTONUM DESC";
            var temEstacionamento = await connection.QueryFirstOrDefaultAsync<DadosAgendamentoModel?>(query, new { placa = placa });

            if (temEstacionamento is not null)
            {
                var queryResult = @"
                    UPDATE 
	                    REDEX..TB_PRE_REGISTRO
                            SET DATA_CHEGADA=@DataChegada,
                                LOCAL=@LocalPatio where autonum=@id";

                var result = await connection.ExecuteAsync(query, new { id = temEstacionamento.AgendamentoId });

                return result > 0;
            }
            else
            {
                var insertQuery = SqlQueries.InsertPreRegistro;

                var result = await connection.ExecuteAsync(query, new { Protocolo = protocolo, 
                                                                        Placa= placa, 
                                                                        Carreta = placaCarreta, 
                                                                        Ticket = ticket, 
                                                                        DataChegada = now1, 
                                                                        LocalPatio = localPatio, 
                                                                        DataChegadaDeicPatio = now2, 
                                                                        FlagDeicPatio = v });

                return result > 0;
            }

            return false;
        }

        public async Task<DadosAgendamentoModel?> PendenciaDeSaidaPatio(string placa)
        {
            using var connection = _connectionFactory.CreateConnection();
            var query = SqlQueries.GetPendenciaSaidaPatioRedex;

            return await connection.QueryFirstOrDefaultAsync<DadosAgendamentoModel?>(query, new { placa = placa });
        }
    }
}
