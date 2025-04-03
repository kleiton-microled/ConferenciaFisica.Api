using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;

namespace ConferenciaFisica.Infra.Repositories
{
    public class MovimentacaoContainerRepository : IMovimentacaoContainerRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public MovimentacaoContainerRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
