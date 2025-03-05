using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using System.ComponentModel;

namespace ConferenciaFisica.Infra.Repositories
{
    public class ConferenciaRepository : IConferenciaRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;

        public ConferenciaRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Conferencia> BuscarPorConteinerAsync(string idConteiner)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.BuscarConferenciaPorIdContainer;

                var ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idConteiner });

                if (ret is null)
                {
                    query = SqlQueries.BuscarConferenciaPorAgendamento;
                    ret = await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idConteiner });
                }

                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<Conferencia> BuscarPorLoteAsync(string idLote)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string query = @"
                SELECT AUTONUM AS Id, BL AS NumeroBl, CNTR AS NumeroCntr, 
                       INICIO, TERMINO, NOME_CLIENTE, CPF_CLIENTE, QTDE_AVARIADA, OBS_AVARIA,
                       DIVERGENCIA_QTDE AS DivergenciaQuantidade, DIVERGENCIA_QUALIFICACAO, OBS_DIVERGENCIA, 
                       RETIRADA_AMOSTRA, 
                       CASE 
                         WHEN NVL(BL, 0) <> 0 THEN 'CARGA SOLTA'
                         WHEN NVL(CNTR, 0) <> 0 THEN 'CONTEINER'
                         ELSE 'REDEX'
                       END AS TipoCarga
                FROM SGIPA.TB_EFETIVACAO_CONF_FISICA
                WHERE BL = :idLote
            ";

            return await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idLote });
        }

        public async Task<Conferencia> BuscarPorReservaAsync(string idLote)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.BuscarConferenciaPorReserva;

            return await connection.QueryFirstOrDefaultAsync<Conferencia>(query, new { idLote });
        }

        public async Task<bool> IniciarConferencia(ConferenciaFisicaCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.InsertConferenciaFisica;

            var ret = await connection.ExecuteAsync(query, command);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AtualizarConferencia(ConferenciaFisicaCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.AtualizarConferencia;

            var ret = await connection.ExecuteAsync(query, command);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CadastroAdicional(CadastroAdicionalCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.CadastroAdicional;

            var ret = await connection.ExecuteAsync(query, command);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<CadastrosAdicionaisDTO>> CarregarCadastrosAdicionais(int idConferencia)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.CarregarCadastrosAdicionais;

                var ret = await connection.QueryAsync<CadastrosAdicionaisDTO>(query, new { idConferencia });


                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<TipoLacre>> CarregarTiposLacres()
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.CarregarTiposLacres;

                var ret = await connection.QueryAsync<TipoLacre>(query);


                return ret;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.ExlcuirCadastroAdicional;

                var ret = await connection.ExecuteAsync(query, new { id });
                if (ret > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }

       
    }
}
