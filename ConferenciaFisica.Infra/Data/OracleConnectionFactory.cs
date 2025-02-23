using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ConferenciaFisica.Infra.Data
{
    public class OracleConnectionFactory
    {
        private readonly string _connectionString;

        public OracleConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDb")
                ?? throw new ArgumentNullException("Connection string não encontrada.");
        }

        public IDbConnection CreateConnection()
        {
            var connection = new OracleConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
