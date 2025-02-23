using ConferenciaFisica.Infra.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;

namespace ConferenciaFisica.Infra.HealthChecks
{
    public class OracleHealthCheck : IHealthCheck
    {
        private readonly OracleConnectionFactory _connectionFactory;

        public OracleHealthCheck(OracleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                if (connection.State == ConnectionState.Open)
                {
                    return HealthCheckResult.Healthy("Conexão com Oracle está ativa.");
                }
                return HealthCheckResult.Unhealthy("Falha ao conectar ao banco de dados.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Erro na conexão com Oracle: {ex.Message}");
            }
        }
    }
}
