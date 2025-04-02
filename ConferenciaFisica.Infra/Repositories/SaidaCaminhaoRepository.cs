using System.Data;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Enums;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;

namespace ConferenciaFisica.Infra.Repositories
{
    public class SaidaCaminhaoRepository : ISaidaDoCaminhaoRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public SaidaCaminhaoRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<RegistroSaidaCaminhaoDTO?> GetDadosCaminhao(GetDadosCaminhaoCommand getDadosCaminhaoCommand)
        {
            try
            {
                var parametros = new DynamicParameters();
                var where = " WHERE 1=1 ";

                if (getDadosCaminhaoCommand.LocalPatio == (int)LocalPatio.Patio)
                {
                    where = " WHERE  (PR.SAIDA_PATIO IS NULL AND PR.DATA_CHEGADA IS NOT NULL)  ";
                }

                if (!string.IsNullOrWhiteSpace(getDadosCaminhaoCommand.Protocolo))
                {
                    var protocoloAno = $"{getDadosCaminhaoCommand.Protocolo}/{getDadosCaminhaoCommand.Ano}";
                    parametros.Add(name: "Protocolo", value: protocoloAno, direction: ParameterDirection.Input);
                    where += " AND PR.Protocolo = @Protocolo";
                }

                if (!string.IsNullOrWhiteSpace(getDadosCaminhaoCommand.Placa))
                {
                    parametros.Add(name: "Placa", value: getDadosCaminhaoCommand.Placa, direction: ParameterDirection.Input);
                    where += " AND PR.PLACA = @Placa";
                }

                if (!string.IsNullOrWhiteSpace(getDadosCaminhaoCommand.PlacaCarreta))
                {
                    parametros.Add(name: "Carreta", value: getDadosCaminhaoCommand.PlacaCarreta, direction: ParameterDirection.Input);
                    where += " AND PR.CARRETA = @Carreta ";
                }

                var query = SqlQueries.BuscarDadosCaminhao;
                var d = System.String.Format(query, where);
                using var connection = _connectionFactory.CreateConnection();

                return connection.Query<RegistroSaidaCaminhaoDTO>(d, parametros).FirstOrDefault();
            }
            catch (Exception exception)
            { 
                Console.WriteLine(exception.Message);
                return null;
            }
        }

        public async Task<bool> RegistrarSaida(RegistrarSaidaCaminhaoCommand registrarSaidaCaminhaoCommand, LocalPatio localPatio)
        {
            try
            {
                var query = string.Empty;

                switch (localPatio)
                {
                    case LocalPatio.Patio:
                        query = SqlQueries.UpdateSaidaCaminhaoPatio;
                        break;
                    case LocalPatio.Estacionamento:
                        query = SqlQueries.UpdateSaidaCaminhaoEstacionamento;
                        break;
                }

                using var connection = _connectionFactory.CreateConnection();

                var result = await connection.ExecuteAsync(query, registrarSaidaCaminhaoCommand);

                return result > 0;
            }
            catch (Exception exception)
            {
                return false;
            }
        }
    }
}
