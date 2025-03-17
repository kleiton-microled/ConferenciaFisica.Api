using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Entities.DescargaExportacao;
using ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using Microsoft.Data.SqlClient;
using System.IO;
using static Dapper.SqlMapper;

namespace ConferenciaFisica.Infra.Repositories.DescargaExportacaoRepository
{
    public class DescargaExportacaoRepository : IDescargaExportacaoRepository
    {
        private readonly SqlServerConnectionFactory _connectionFactory;
        public DescargaExportacaoRepository(SqlServerConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<DescargaExportacao> BuscarRegistroAsync(int registro)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                string command = SqlQueries.CarregarRegistro;

                var registroDict = new Dictionary<int, DescargaExportacao>();
                var result = await connection.QueryAsync<DescargaExportacao, Talie, TalieItem, DescargaExportacao>(
                command,
                (registro, talie, talieItem) =>
                    {
                        if (!registroDict.TryGetValue(registro.Id, out var registroEntry))
                        {
                            registroEntry = registro;
                            registroEntry.Talie = talie;
                            if (registroEntry.Talie != null)
                                registroEntry.Talie.TalieItem = new List<TalieItem>();

                            registroDict.Add(registro.Id, registroEntry);
                        }

                        if (talieItem != null)
                        {
                            registroEntry.Talie.TalieItem.Add(talieItem);
                        }

                        return registroEntry;
                    },
                     new { registro },
                    splitOn: "Id,Id"
                );

                return registroDict.Values.FirstOrDefault();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<int> AtualizarOuCriarTalie(DescargaExportacaoCommand command)
        {
            using var connection = _connectionFactory.CreateConnection();

            string verificaTalieSql = @"
                SELECT COUNT(*) 
                FROM REDEX..tb_talie 
                WHERE autonum_reg = @CodigoRegistro"
            ;

            int count = connection.ExecuteScalar<int>(verificaTalieSql, new { CodigoRegistro = command.Registro });

            if (count > 0)
            {
                string updateSql = SqlQueries.AtualizarTalie;

                var talieId = await connection.QueryFirstOrDefaultAsync<int>(updateSql, new
                {
                    Conferente = command.Talie.Conferente,
                    Equipe = command.Talie.Equipe,
                    Operacao = command.Talie.Operacao,
                    CodigoRegistro = command.Registro
                });



                return talieId;
            }
            else
            {

                string insertSql = SqlQueries.CriarTalie;

                int talieId = await connection.ExecuteScalarAsync<int>(insertSql, new
                {
                    Placa = command.Placa,
                    Inicio = DateTime.Now,
                    Conferente = command.Talie?.Conferente,
                    Equipe = command.Talie?.Equipe,
                    CodigoRegistro = command.Registro
                });

                return talieId;
            }

        }

        public async Task<IEnumerable<TalieItem>> BuscarTaliItens(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = SqlQueries.ListaTalieItens;
            return await connection.QueryAsync<TalieItem>(sql, new { TalieId = id });
        }

        public async Task GerarDescargaAutomatica(int registro, int talieId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            string queryDescarga = SqlQueries.Descarga;

            var registros = await connection.QueryAsync(queryDescarga, new { CodigoRegistro = registro });

            foreach (var reg in registros)
            {
                long idNF = 0;
                double pesoNF = 0;

                // Verifica se a NF já está cadastrada
                if (!string.IsNullOrEmpty(reg.DANFE))
                {
                    string queryNF = @"SELECT MAX(AUTONUM_NF) AS AUTONUM_NF 
                                        FROM REDEX..tb_NOTAS_FISCAIS 
                                      WHERE DANFE = @DANFE";

                    var idNFResult = connection.ExecuteScalar<long?>(queryNF, new { DANFE = reg.DANFE });
                    idNF = (idNFResult.HasValue ? idNFResult.Value : 0);
                }

                // Define o peso da NF
                if (idNF != 0)
                {
                    string queryPesoNF = @"SELECT ISNULL(peso_bruto, 0) 
                                            FROM REDEX..tb_notas_fiscais 
                                           WHERE autonum_nf = @IdNF";

                    var pesoNFResult = connection.ExecuteScalar<double?>(queryPesoNF, new { IdNF = idNF });
                    pesoNF = pesoNFResult.HasValue ? pesoNFResult.Value : 0;
                }
                else
                {
                    pesoNF = reg.peso_bruto ?? 0;
                }

                // Inserir na tabela de itens da descarga
                string insertItem = @"
                                        INSERT INTO REDEX..tb_talie_item (
                                            autonum_talie, autonum_regcs, qtde_descarga, tipo_descarga, 
                                            diferenca, obs, qtde_disponivel, comprimento, largura, altura, peso, 
                                            qtde_estufagem, marca, remonte, fumigacao, flag_fragil, flag_madeira, 
                                            YARD, armazem, autonum_nf, nf, imo, uno, imo2, uno2, imo3, uno3, imo4, uno4, 
                                            autonum_emb, autonum_pro
                                        ) VALUES (
                                            @AutonumTalie, @AutonumRegcs, @QtdeDescarga, 'TOTAL', 
                                            '0', '', 0, 0, 0, 0, @Peso, 0, '', 0, '', 0, 0, NULL, NULL, 
                                            @IdNF, @NF, @IMO, @UNO, @IMO2, @UNO2, @IMO3, @UNO3, @IMO4, @UNO4, 
                                            @AutonumEmb, @AutonumPro
                                        )";


                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("AutonumTalie", talieId);
                parameters.Add("AutonumRegcs", reg.AUTONUM_REGCS);
                parameters.Add("QtdeDescarga", reg.QUANTIDADE);
                parameters.Add("Peso", pesoNF);
                parameters.Add("IdNF", idNF);
                parameters.Add("NF", reg.NF);
                parameters.Add("IMO", reg.IMO);
                parameters.Add("UNO", reg.UNO);
                parameters.Add("IMO2", reg.IMO2);
                parameters.Add("UNO2", reg.UNO2);
                parameters.Add("IMO3", reg.IMO3);
                parameters.Add("UNO3", reg.UNO3);
                parameters.Add("IMO4", reg.IMO4);
                parameters.Add("UNO4", reg.UNO4);
                parameters.Add("AutonumEmb", reg.autonum_emb);
                parameters.Add("AutonumPro", reg.autonum_pro);

                connection.Execute(insertItem, parameters);
            }
        }

        public async Task<int> CadastrarAvaria(CadastroAvariaCommand command)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;

            await using var transaction = await connection.BeginTransactionAsync();
            try
            {
                // Removendo avarias anteriores
                string deleteAvarias = SqlQueries.ExcluirAvarias;
                await connection.ExecuteAsync(deleteAvarias, command, transaction);

                // Inserindo novas avarias (uso de Bulk Insert para maior eficiência)
                string insertQuery = SqlQueries.CadastrarAvariaCs;
                Random random = new Random();

                if (command.TiposAvarias?.Any() == true)
                {
                    var parameters = command.TiposAvarias.Select(tipo => new
                    {
                        command.TalieId,
                        command.Local,
                        flagDivergencia = command.Divergencia,
                        command.QuantidadeAvariada,
                        command.PesoAvariado,
                        command.Observacao,
                        autonumAvcs = random.Next(100000, 999999),
                        tipo = tipo.Id
                    }).ToList();

                    await connection.ExecuteAsync(insertQuery, parameters, transaction);
                }

                await transaction.CommitAsync();
                return command.TalieId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar AvariaConferencia: {ex.Message}", ex);
            }
        }


        public Task<int> AtualizarAvaria(CadastroAvariaCommand command)
        {
            throw new NotImplementedException();
        }

        public async Task<TalieItem> BuscarTalieItem(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.BuscarTaliItemPorId;

            var ret = await connection.QuerySingleAsync<TalieItem>(query, new { id });

            return ret;
        }

        public async Task<bool> UpdateTalieItem(TalieItem item)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.AtualizarTalieItem;

            var parameters = new DynamicParameters();
            // Adiciona os parâmetros
            parameters.Add("@TalieItemId", item.Id);
            parameters.Add("@QtdDescarga", item.QtdDescarga);
            parameters.Add("@IdEmbalagem", item.CodigoEmbalagem);
            parameters.Add("@Comprimento", item.Comprimento);
            parameters.Add("@Altura", item.Altura); // Corrigido para 'Altura'
            parameters.Add("@Largura", item.Largura); // Corrigido para 'Largura'
            parameters.Add("@Peso", item.Peso);
            parameters.Add("@Imo", item.IMO);
            parameters.Add("@Imo2", item.IMO2);
            parameters.Add("@Imo3", item.IMO3);
            parameters.Add("@Imo4", item.IMO4);
            parameters.Add("@Imo5", item.IMO5);
            parameters.Add("@Uno", item.UNO);
            parameters.Add("@Uno2", item.UNO2);
            parameters.Add("@Uno3", item.UNO3);
            parameters.Add("@Uno4", item.UNO4);
            parameters.Add("@Uno5", item.UNO5);

            var ret = await connection.ExecuteAsync(query, parameters);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public async Task<bool> CadastrarTalieItem(TalieItem item, int registro)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.AtualizarTalieItem;

            var parameters = new DynamicParameters();

            string queryDescarga = @"
                                        SELECT 
                                            rcs.*, 
                                            bcg.qtde AS qtde_manifestada, 
                                            ISNULL(bcg.peso_bruto, 0) / NULLIF(bcg.qtde, 0) AS peso_manifestado,
                                            bcg.imo, bcg.imo2, bcg.imo3, bcg.imo4, 
                                            bcg.uno, bcg.uno2, bcg.uno3, bcg.uno4, 
                                            bcg.autonum_pro, bcg.autonum_emb
                                        FROM REDEX..tb_registro reg
                                        INNER JOIN REDEX..tb_registro_cs rcs ON reg.autonum_reg = rcs.autonum_reg
                                        INNER JOIN REDEX..tb_booking_carga bcg ON rcs.autonum_bcg = bcg.autonum_bcg
                                        WHERE reg.autonum_reg = @CodigoRegistro";

            var reg = connection.Query(queryDescarga, new { CodigoRegistro = registro }).ToList().FirstOrDefault();

            string insertItem = @"
                                        INSERT INTO REDEX..tb_talie_item (
                                            autonum_talie, autonum_regcs, qtde_descarga, tipo_descarga, 
                                            diferenca, obs, qtde_disponivel, comprimento, largura, altura, peso, 
                                            qtde_estufagem, marca, remonte, fumigacao, flag_fragil, flag_madeira, 
                                            YARD, armazem, autonum_nf, nf, imo, uno, imo2, uno2, imo3, uno3, imo4, uno4, 
                                            autonum_emb, autonum_pro
                                        ) VALUES (
                                            @AutonumTalie, @AutonumRegcs, @QtdDescarga, 'PARCIAL', 
                                            '0', '', 0, @comprimento, @largura, @altura, @Peso, 0, '', @remonte, @fumigacao, @flagfragil, @flagmadeira, NULL, NULL, 
                                            @NfId, @NF, @IMO, @UNO, @IMO2, @UNO2, @IMO3, @UNO3, @IMO4, @UNO4, 
                                            @AutonumEmb, @AutonumPro
                                        )";


            parameters.Add("AutonumTalie", item.TalieId);
            parameters.Add("AutonumRegcs", reg.AUTONUM_REGCS);
            parameters.Add("@QtdDescarga", item.QtdDescarga);
            parameters.Add("@Peso", item.Peso);
            parameters.Add("@NfId", item.NotaFiscalId);
            parameters.Add("@Nf", item.NotaFiscal);
            parameters.Add("@Comprimento", item.Comprimento);
            parameters.Add("@Largura", item.Altura);
            parameters.Add("@Altura", item.Largura);
            parameters.Add("@Imo", item.IMO);
            parameters.Add("@Imo2", item.IMO2);
            parameters.Add("@Imo3", item.IMO3);
            parameters.Add("@Imo4", item.IMO4);
            parameters.Add("@Uno", item.UNO);
            parameters.Add("@Uno2", item.UNO2);
            parameters.Add("@Uno3", item.UNO3);
            parameters.Add("@Uno4", item.UNO4);
            parameters.Add("@Remonte", item.Remonte);
            parameters.Add("@Fumigacao", item.Fumigacao);
            parameters.Add("@FlagMadeira", item.Madeira);
            parameters.Add("@FlagFragil", item.Fragil);
            parameters.Add("AutonumEmb", item.CodigoEmbalagem);
            parameters.Add("AutonumPro", reg.autonum_pro);

            var ret = await connection.ExecuteAsync(insertItem, parameters);
            if (ret > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ExcluirTalieItem(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.ExcluirTalieItem;

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

        public async Task<bool> GravarObservacao(string observacao, int talieId)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.GravarObservacao;

                var ret = await connection.ExecuteAsync(query, new { observacao, talieId });
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

        public async Task<IEnumerable<Armazens>> CarregarArmazens(int patio)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.ListarArmazensPorPatio;

            var ret = await connection.QueryAsync<Armazens>(query, new { patio });

            return ret;
        }

        public async Task<bool> GravarMarcante(MarcanteCommand command)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.GravarMarcante;
                DynamicParameters param = new DynamicParameters();
                param.Add("armazem", command.Armazem);
                //param.Add("placa", command.Placa);
                param.Add("talieId", command.TalieId);
                param.Add("talieItemId", command.TalieItemId);
                param.Add("idRegistro", command.Registro);
                param.Add("id", command.Marcante);

                var ret = await connection.ExecuteAsync(query, param);
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

        public async Task<IEnumerable<Marcante>> CarregarMarcantesTalieItem(int talieItemId)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.CarregarMarcantesTalieItem;

            var ret = await connection.QueryAsync<Marcante>(query, new { talieItemId });

            return ret;
        }

        public async Task<bool> ExcluirMarcanteTalieItem(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                string query = SqlQueries.ExcluirMarcanteTalieItem;

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

        public Task<bool> FinalizarProcesso(int id)
        {
            throw new NotImplementedException();
        }
    }
}
