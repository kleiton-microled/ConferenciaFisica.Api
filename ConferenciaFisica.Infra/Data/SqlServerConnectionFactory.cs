using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using ConferenciaFisica.Application.Interfaces;

namespace ConferenciaFisica.Infra.Data
{
    public class SqlServerConnectionFactory
    {
        private readonly string _baseConnectionString;
        private readonly ILogger<SqlServerConnectionFactory> _logger;
        private readonly ISchemaService _schemaService;

        public SqlServerConnectionFactory(IConfiguration configuration, ILogger<SqlServerConnectionFactory> logger, ISchemaService schemaService)
        {
            _baseConnectionString = configuration.GetConnectionString("SqlServerDb")
                ?? throw new ArgumentNullException("Connection string não encontrada.");

            _logger = logger;
            _schemaService = schemaService;
        }

        public IDbConnection CreateConnection()
        {
            try
            {
                var dynamicConnectionString = _schemaService.GetConnectionString(_baseConnectionString);
                var currentDatabase = _schemaService.GetCurrentDatabase();
                
                _logger.LogInformation($"Tentando abrir conexão com SQL Server no banco {currentDatabase}...");
                var connection = new SqlConnection(dynamicConnectionString);
                connection.Open(); // Tenta abrir a conexão

                _logger.LogInformation($"Conexão estabelecida com sucesso no banco {currentDatabase}.");
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

        /// <summary>
        /// Cria e abre uma conexão assíncrona com o banco de dados.
        /// </summary>
        public async Task<IDbConnection> CreateConnectionAsync()
        {
            try
            {
                var dynamicConnectionString = _schemaService.GetConnectionString(_baseConnectionString);
                var currentDatabase = _schemaService.GetCurrentDatabase();
                
                _logger.LogInformation($"Tentando abrir conexão assíncrona com SQL Server no banco {currentDatabase}...");
                var connection = new SqlConnection(dynamicConnectionString);
                await connection.OpenAsync(); // Abre a conexão de forma assíncrona

                _logger.LogInformation($"Conexão assíncrona estabelecida com sucesso no banco {currentDatabase}.");
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao conectar ao banco de dados: {ex.Message}");
                throw new ApplicationException("Erro ao conectar ao banco de dados.");
            }
        }

        /// <summary>
        /// Inicia uma transação de forma síncrona.
        /// </summary>
        public IDbTransaction BeginTransaction(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("A conexão deve estar aberta antes de iniciar uma transação.");
            }

            _logger.LogInformation("Iniciando transação...");
            return connection.BeginTransaction();
        }

        /// <summary>
        /// Inicia uma transação de forma assíncrona.
        /// </summary>
        public async Task<IDbTransaction> BeginTransactionAsync(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
            {
                throw new InvalidOperationException("A conexão deve estar aberta antes de iniciar uma transação.");
            }

            _logger.LogInformation("Iniciando transação assíncrona...");
            return await Task.FromResult(connection.BeginTransaction());
        }
        
        /// <summary>
        /// Cria uma conexão específica para lotes (sempre SGIPA)
        /// </summary>
        public IDbConnection CreateLotesConnection()
        {
            try
            {
                var lotesConnectionString = _schemaService.GetLotesConnectionString(_baseConnectionString);
                
                _logger.LogInformation("Tentando abrir conexão com SQL Server no banco SGIPA para lotes...");
                var connection = new SqlConnection(lotesConnectionString);
                connection.Open();

                _logger.LogInformation("Conexão para lotes estabelecida com sucesso no banco SGIPA.");
                return connection;
            }
            catch (SqlException ex) when (ex.Number == -2) // Timeout
            {
                _logger.LogError($"Erro de timeout ao conectar ao banco SGIPA para lotes: {ex.Message}");
                throw new ApplicationException("A conexão com o banco de dados SGIPA para lotes expirou. Tente novamente mais tarde.");
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Erro de conexão com o banco SGIPA para lotes: {ex.Message}");
                throw new ApplicationException("Erro ao conectar ao banco SGIPA para lotes. Verifique sua conexão e tente novamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado ao conectar ao banco SGIPA para lotes: {ex.Message}");
                throw new ApplicationException("Ocorreu um erro inesperado ao conectar ao banco SGIPA para lotes.");
            }
        }
        
        /// <summary>
        /// Cria uma conexão assíncrona específica para lotes (sempre SGIPA)
        /// </summary>
        public async Task<IDbConnection> CreateLotesConnectionAsync()
        {
            try
            {
                var lotesConnectionString = _schemaService.GetLotesConnectionString(_baseConnectionString);
                
                _logger.LogInformation("Tentando abrir conexão assíncrona com SQL Server no banco SGIPA para lotes...");
                var connection = new SqlConnection(lotesConnectionString);
                await connection.OpenAsync();

                _logger.LogInformation("Conexão assíncrona para lotes estabelecida com sucesso no banco SGIPA.");
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao conectar ao banco SGIPA para lotes: {ex.Message}");
                throw new ApplicationException("Erro ao conectar ao banco SGIPA para lotes.");
            }
        }
    }
}
