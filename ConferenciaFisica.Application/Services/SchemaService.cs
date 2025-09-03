using ConferenciaFisica.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ConferenciaFisica.Application.Services
{
    public class SchemaService : ISchemaService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string DEFAULT_SCHEMA = "SGIPA";
        private const string REDEX_SCHEMA = "REDEX";
        private const string SGIPA_SCHEMA = "SGIPA";

        public SchemaService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentSchema()
        {
            var environment = GetCurrentEnvironment();
            return environment?.ToUpper() switch
            {
                REDEX_SCHEMA => REDEX_SCHEMA,
                SGIPA_SCHEMA => SGIPA_SCHEMA,
                _ => DEFAULT_SCHEMA
            };
        }

        public void SetEnvironment(string environment)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Items["CurrentEnvironment"] = environment;
            }
        }

        public string GetTableName(string tableName)
        {
            var schema = GetCurrentSchema();
            return $"{schema}.dbo.{tableName}";
        }

        public string GetCurrentEnvironment()
        {
            if (_httpContextAccessor.HttpContext?.Items.TryGetValue("CurrentEnvironment", out var environment) == true)
            {
                return environment?.ToString();
            }

            // Fallback para o header da requisição
            var headerEnvironment = _httpContextAccessor.HttpContext?.Request.Headers["X-Environment"].FirstOrDefault();
            if (!string.IsNullOrEmpty(headerEnvironment))
            {
                return headerEnvironment;
            }

            return DEFAULT_SCHEMA;
        }
        
        public string GetCurrentDatabase()
        {
            var environment = GetCurrentEnvironment();
            return environment?.ToUpper() switch
            {
                REDEX_SCHEMA => "REDEX",
                SGIPA_SCHEMA => "SGIPA",
                _ => "SGIPA" // Padrão
            };
        }
        
        public string GetConnectionString(string baseConnectionString)
        {
            var currentDatabase = GetCurrentDatabase();
            
            // Substitui o parâmetro Database na connection string
            if (baseConnectionString.Contains("Database="))
            {
                // Substitui Database=SGIPA por Database=REDEX ou vice-versa
                var regex = new System.Text.RegularExpressions.Regex(@"Database=[^;]+");
                return regex.Replace(baseConnectionString, $"Database={currentDatabase}");
            }
            
            return baseConnectionString;
        }
        
        public string GetLotesDatabase()
        {
            // Lotes sempre usam SGIPA, independente do ambiente
            return "SGIPA";
        }
        
        public string GetLotesConnectionString(string baseConnectionString)
        {
            // Para lotes, sempre força o uso do banco SGIPA
            if (baseConnectionString.Contains("Database="))
            {
                var regex = new System.Text.RegularExpressions.Regex(@"Database=[^;]+");
                return regex.Replace(baseConnectionString, "Database=SGIPA");
            }
            
            return baseConnectionString;
        }
    }
}
