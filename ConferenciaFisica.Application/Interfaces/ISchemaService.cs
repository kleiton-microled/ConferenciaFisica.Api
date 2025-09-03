namespace ConferenciaFisica.Application.Interfaces
{
    public interface ISchemaService
    {
        /// <summary>
        /// Obtém o schema atual baseado no ambiente
        /// </summary>
        string GetCurrentSchema();
        
        /// <summary>
        /// Define o ambiente atual (REDEX ou SGIPA)
        /// </summary>
        void SetEnvironment(string environment);
        
        /// <summary>
        /// Obtém o nome completo da tabela com schema
        /// </summary>
        string GetTableName(string tableName);
        
        /// <summary>
        /// Obtém o ambiente atual
        /// </summary>
        string GetCurrentEnvironment();
        
        /// <summary>
        /// Obtém a string de conexão modificada para o ambiente atual
        /// </summary>
        string GetConnectionString(string baseConnectionString);
        
        /// <summary>
        /// Obtém o nome do banco de dados atual
        /// </summary>
        string GetCurrentDatabase();
        
        /// <summary>
        /// Obtém o nome do banco de dados para lotes (sempre SGIPA)
        /// </summary>
        string GetLotesDatabase();
        
        /// <summary>
        /// Obtém a string de conexão para lotes (sempre SGIPA)
        /// </summary>
        string GetLotesConnectionString(string baseConnectionString);
    }
}
