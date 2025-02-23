﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ConferenciaFisica.Infra.Data
{
    public class SqlServerConnectionFactory
    {
        private readonly string _connectionString;
        private readonly ILogger<SqlServerConnectionFactory> _logger;

        public SqlServerConnectionFactory(IConfiguration configuration, ILogger<SqlServerConnectionFactory> logger)
        {
            _connectionString = configuration.GetConnectionString("SqlServerDb")
                ?? throw new ArgumentNullException("Connection string não encontrada.");

            _logger = logger;
        }

        public IDbConnection CreateConnection()
        {
            try
            {
                _logger.LogInformation("Tentando abrir conexão com SQL Server...");
                var connection = new SqlConnection(_connectionString);
                connection.Open(); // Tenta abrir a conexão

                _logger.LogInformation("Conexão estabelecida com sucesso.");
                return connection;
            }
            catch (SqlException ex) when (ex.Number == -2) // Timeout
            {
                _logger.LogError($"Erro de timeout ao conectar ao banco: {ex.Message}");
                throw new ApplicationException("A conexão com o banco de dados expirou. Tente novamente mais tarde.");
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Erro de conexão com o banco de dados: {ex.Message}");
                throw new ApplicationException("Erro ao conectar ao banco de dados. Verifique sua conexão e tente novamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado ao conectar ao banco: {ex.Message}");
                throw new ApplicationException("Ocorreu um erro inesperado ao conectar ao banco de dados.");
            }
        }
    }
}
