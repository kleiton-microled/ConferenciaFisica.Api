using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using System.Data;

namespace ConferenciaFisica.Infra.Repositories
{
    public class MovimentacaoCargaSoltaRepository : IMovimentacaoCargaSoltaRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public MovimentacaoCargaSoltaRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<MovimentacaoCargaDTO> BuscarCargaParaMovimentar(int idMarcante)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = SqlQueries.BuscarCargaParaMovimentar;
            return await connection.QueryFirstOrDefaultAsync<MovimentacaoCargaDTO>(sql, new { idMarcante });
        }

        public async Task<bool> Movimentar(MovimentacaoCargaDTO carga)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = SqlQueries.MovimentarCarga;

            var result = await connection.ExecuteScalarAsync<int>(
                sql,
                new
                {
                    idMarcante = carga.IdMarcante,
                    etiquetaPrateleira = carga.EtiquetaPrateleira,
                    armazem = carga.Armazem,
                    local = carga.Local
                },
                commandType: CommandType.StoredProcedure
            );

            return result == 1;
        }

    }
}
