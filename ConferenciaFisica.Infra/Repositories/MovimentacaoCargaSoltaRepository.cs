using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using Microsoft.Data.SqlClient;
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
            var slqInsertYard = SqlQueries.Insert_TB_CARGA_SOLTA_YARD;
            var sql = SqlQueries.MovimentarCarga;
            //
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            await using var transaction = await connection.BeginTransactionAsync();

            var autonumPcs = await connection.QueryFirstOrDefaultAsync<int>(SqlQueries.BuscarIdPatioCs, new {idRegistro = carga.Registro}, transaction);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("autonumPatioCs", autonumPcs);
            parameters.Add("armazem", carga.Armazem);
            parameters.Add("yard", carga.Local);
            parameters.Add("quantidade", carga.Quantidade);
            parameters.Add("motivo", carga.Motivo);
            parameters.Add("usuario", 1);

            try
            {
                var insertYard = await connection.ExecuteScalarAsync<int>(slqInsertYard, parameters, transaction);

                var result = await connection.ExecuteScalarAsync<int>(
                 sql,
                 new
                 {
                     idMarcante = carga.IdMarcante,
                     etiquetaPrateleira = carga.EtiquetaPrateleira,
                     armazem = carga.Armazem,
                     local = carga.Local
                 },
                 transaction,
                 commandType: CommandType.StoredProcedure
             );

                await connection.ExecuteAsync(SqlQueries.UpdateMarcante, new { autonumCsYard= insertYard, idRegistro = carga.Registro, codigoMarcante = carga.IdMarcante}, transaction);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); 
                throw ex;
                
            }


        }

    }
}
