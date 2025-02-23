using ConferenciaFisica.Infra.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;

namespace ConferenciaFisica.Infra.HealthChecks
{
    public class SqlServerHealthCheck : IHealthCheck
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public SqlServerHealthCheck(SqlServerConnectionFactory connectionFactory)
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
                    return HealthCheckResult.Healthy("Conexão com SQL Server está ativa.");
                }
                return HealthCheckResult.Unhealthy("Falha ao conectar ao banco de dados SQL Server.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Erro na conexão com SQL Server: {ex.Message}");
            }
        }
    }
}
