using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Contracts.DTOs.FinalizarProcesso;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Entities.DescargaExportacao;
using ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory;
using ConferenciaFisica.Infra.Data;
using ConferenciaFisica.Infra.Sql;
using Dapper;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
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

        public async Task<IEnumerable<PatioCsCrossDock>> BuscarTalieCrossDock(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = SqlQueries.CrossDockBuscarInfoTalie;

            var ret = await connection.QueryAsync<PatioCsCrossDock>(query, new { talieId = id });

            return ret;
        }

        public async Task<int?> CrossDockGetNumeroReservaContainer(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.CrossDockBuscarInfoTalie;

                var ret = await connection.QuerySingleAsync<int?>(query, new { talieId = id });

                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;    
            }
        }

        public async Task<int> GetCrossDockRomaneioId(int id)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var query = SqlQueries.CrossDockGetRomaneioId;

                var ret = await connection.QuerySingleAsync<int>(query);

                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<bool> CrossDockUpdatePatioF(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = SqlQueries.CrossDockSetPatioToF;

            var result = await connection.ExecuteAsync(query, new {patioId = id});

            return result > 0;
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
                    CodigoRegistro = command.Registro,
                    Termino = command.Talie.Termino
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
                    Conferente = 1,
                    Equipe = 1,
                    CodigoRegistro = command.Registro,
                    IdReserva = 365016,
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
            parameters.Add("@Observacao", item.Observacao);
            parameters.Add("@Fragil", item.Fragil);
            parameters.Add("@Madeira", item.Madeira);
            parameters.Add("@Remonte", item.Remonte);

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

            var reg = connection.Query(queryDescarga, new { CodigoRegistro = registro }).Where(x=> x.NF == item.NotaFiscal).FirstOrDefault();

            string insertItem = SqlQueries.AtualizarAvariaConferencia;


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
                param.Add("yard", command.Local);
                param.Add("talieId", command.TalieId);
                param.Add("talieItemId", command.TalieItemId);
                param.Add("idRegistro", command.Registro);
                param.Add("codigoMarcante", command.Marcante);
                param.Add("quantidade", command.Quantidade);

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

        public async Task<bool> FinalizarProcesso(int id)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            using var transaction = await connection.BeginTransactionAsync();


            try
            {
                var data = await ObterFinalizarDescargaAsync(id);

                foreach (var registro in data.Item)
                {
                    // 🔹 Gerando novo ID para TB_PATIO_CS
                    int novoId = await ObterProximoIdPatioCsAsync();

                    // 🔹 Inserindo na tabela TB_PATIO_CS
                    string sqlInsertPatio = SqlQueries.InsertIntoTbPatioCs;

                    #region PARAMETERS
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("id", novoId);
                    parameters.Add("AutonumBcg", registro.IdBookingCarga);//TODO, vem da tabela TB_REGISTRO_CS - 359731
                    parameters.Add("QuantidadeEntrada", registro.Quantidade);
                    parameters.Add("AutonumEmb", registro.Embalagem);
                    parameters.Add("AutonumPro", registro.Produto);
                    parameters.Add("Marca", registro.Marca);
                    parameters.Add("VolumeDeclarado", 0); //TODO - Calculo altura x largura x comprimento CLA
                    parameters.Add("Comprimento", registro.Comprimento);
                    parameters.Add("Largura", registro.Largura);
                    parameters.Add("Altura", registro.Altura);
                    parameters.Add("Bruto", registro.Peso);
                    parameters.Add("QtdeUnit", registro.Quantidade);
                    parameters.Add("DataRegistro", data.DataTermino);
                    parameters.Add("AutonumRegcs", registro.RegistroCargaSolta);
                    parameters.Add("AutonumNf", registro.NotaFiscal);
                    parameters.Add("AutonumTi", registro.Id);
                    parameters.Add("QtdeEstufagem", registro.QuantidadeEstufagem);
                    parameters.Add("Yard", registro.Yard);
                    parameters.Add("Armazem", registro.Armazem);
                    parameters.Add("AutonumPatios", 2);
                    parameters.Add("Patio", 2);
                    parameters.Add("Imo", registro.IMO);
                    parameters.Add("Imo2", registro.IMO2);
                    parameters.Add("Imo3", registro.IMO3);
                    parameters.Add("Imo4", registro.IMO4);
                    parameters.Add("Uno", registro.UNO);
                    parameters.Add("Uno2", registro.UNO2);
                    parameters.Add("Uno3", registro.UNO3);
                    parameters.Add("Uno4", registro.UNO4);
                    parameters.Add("CodProduto", registro.CodigoProduto);
                    parameters.Add("CodEan", registro.CodigoEan);
                    parameters.Add("Termino", DateTime.Now);
                    #endregion PARAMETERS

                    await connection.ExecuteAsync(sqlInsertPatio, parameters, transaction);

                    // 🔹 Atualizando IMO na TB_BOOKING_CARGA se houver
                    if (!string.IsNullOrEmpty(registro.IMO))
                    {
                        await AtualizarIMOAsync(registro.IdBookingCarga, registro.IMO);
                    }

                    // 🔹 Inserindo na tabela TB_CARGA_SOLTA_YARD
                    await InserirCargaSoltaYardAsync(registro.RegistroCargaSolta, registro.Armazem, registro.Yard);

                    // 🔹 Gerando novo ID para TB_AMR_GATE
                    long novoIdAmr = await ObterProximoIdAmrAsync();

                    // 🔹 Inserindo na tabela TB_AMR_GATE
                    string sqlInsertAmrGate = @"
                                                INSERT INTO REDEX.dbo.TB_AMR_GATE (
                                                    autonum, gate, cntr_rdx, cs_rdx, peso_entrada, peso_saida, data, id_booking, id_oc, funcao_gate, flag_historico
                                                ) VALUES (
                                                    @IdAmr, @Gate, 0, @Id, @PesoEntrada, 0, @DataRegistro, @AutonumBoo, 0, 203, 0
                                                )";

                    DynamicParameters param = new DynamicParameters();
                    param.Add("IdAmr", novoIdAmr);
                    param.Add("Gate", data.Gate);
                    param.Add("Id", novoId);
                    param.Add("PesoEntrada", registro.Peso);
                    param.Add("DataRegistro", DateTime.Now); //todo
                    param.Add("AutonumBoo", registro.IdBookingCarga); //todo

                    await connection.ExecuteAsync(sqlInsertAmrGate, param, transaction);

                    // 🔹 Atualizando TB_PATIO_CS
                    string sqlUpdatePatio = "UPDATE REDEX.dbo.TB_PATIO_CS SET pcs_pai = @Id WHERE autonum_pcs = @Id";
                    await connection.ExecuteAsync(sqlUpdatePatio, new { Id = novoId }, transaction);

                    //TOUpdate TB_MARCANTE_RDX set Autonum_pcs where autonum_ti = @Id
                    await connection.ExecuteAsync("Update REDEX.dbo.TB_MARCANTES_RDX set Autonum_pcs = @Id where autonum_ti = @idTalieItem", 
                        new {id= novoId, idTalieItem = registro.Id }, transaction);
                }

                await transaction.CommitAsync();
                return true; // ✅ Sucesso
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao processar fechamento do Talie: {ex.Message}", ex);
            }
        }

        public async Task<bool> ValidarQuantidadeDescargaAsync(int talieId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            // 🔹 Soma da quantidade descarregada (qtde_descarga)
            string sqlQuantidadeDescarregada = @"SELECT SUM(qtde_descarga) 
                                                    FROM REDEX.dbo.TB_TALIE_ITEM 
                                                WHERE autonum_talie = @TalieId";

            int quantidadeDescarregada = await connection.ExecuteScalarAsync<int>(sqlQuantidadeDescarregada, new { TalieId = talieId });

            // 🔹 Soma da quantidade registrada (quantidade) na tabela de registro
            string sqlQuantidadeRegistrada = @"SELECT SUM(r.quantidade) 
                                                FROM REDEX.dbo.tb_talie t
                                                INNER JOIN REDEX.dbo.TB_REGISTRO_CS r ON t.autonum_reg = r.autonum_reg
                                               WHERE t.autonum_talie = @TalieId";

            int quantidadeRegistrada = await connection.ExecuteScalarAsync<int>(sqlQuantidadeRegistrada, new { TalieId = talieId });

            // 🔹 Validação: Quantidade Registrada == Quantidade Descarregada
            if (quantidadeDescarregada != quantidadeRegistrada)
            {
                return false;
            }

            return true; // ✅ Tudo certo, não há divergências
        }

        public async Task<int> ObterProximoIdPatioCsAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            string sql = @"
                INSERT INTO REDEX.dbo.SEQ_TB_PATIO_CS (DATA)
                OUTPUT inserted.AUTONUM
                VALUES (GETDATE())";

            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<int> ObterProximoIdAmrAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            string sql = @"
                INSERT INTO REDEX.dbo.seq_tb_amr_gate (DATA)
                OUTPUT inserted.AUTONUM
                VALUES (GETDATE())";

            return await connection.ExecuteScalarAsync<int>(sql);
        }

        public async Task<bool> VerificarEmissaoEtiquetasAsync(int talieId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            // 🔹 Verifica se há etiquetas geradas para o registro
            string sqlEtiquetasGeradas = @"SELECT COUNT(*) 
                                            FROM REDEX.dbo.TB_TALIE t
                                            INNER JOIN REDEX.dbo.TB_TALIE_ITEM ti ON t.autonum_talie = ti.autonum_talie
                                            INNER JOIN REDEX.dbo.ETIQUETAS e ON ti.autonum_regcs = e.autonum_rcs
                                            WHERE t.autonum_talie = @TalieId";

            int etiquetasGeradas = await connection.ExecuteScalarAsync<int>(sqlEtiquetasGeradas, new { TalieId = talieId });

            if (etiquetasGeradas != 0)
            {
                throw new Exception("Não consta geração de etiquetas deste registro. Deseja continuar assim mesmo?");
            }

            // 🔹 Verifica se há etiquetas pendentes de emissão
            string sqlEtiquetasPendentes = @"SELECT COUNT(*) 
                                                FROM REDEX.dbo.TB_TALIE t
                                                INNER JOIN REDEX.dbo.TB_TALIE_ITEM ti ON t.autonum_talie = ti.autonum_talie
                                                INNER JOIN REDEX.dbo.ETIQUETAS e ON ti.autonum_regcs = e.autonum_rcs
                                                WHERE t.autonum_talie = @TalieId 
                                             AND e.emissao IS NULL";

            int etiquetasPendentes = await connection.ExecuteScalarAsync<int>(sqlEtiquetasPendentes, new { TalieId = talieId });

            if (etiquetasPendentes > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<FinalizarDescargaDTO> ObterFinalizarDescargaAsync(int talieId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            string sql = SqlQueries.BuscarRegistroDescarga;

            var resultado = new Dictionary<int, FinalizarDescargaDTO>();

            var data = await connection.QueryAsync<FinalizarDescargaDTO, ItemDTO, FinalizarDescargaDTO>(
                sql,
                (descarga, item) =>
                {
                    if (!resultado.TryGetValue(descarga.Id, out var descargaExistente))
                    {
                        descargaExistente = descarga;
                        descargaExistente.Item = new List<ItemDTO>();
                        resultado.Add(descarga.Id, descargaExistente);
                    }

                    if (item != null)
                    {
                        descargaExistente.Item.Add(item);
                    }

                    return descargaExistente;
                },
                new { TalieId = talieId },
                splitOn: "Id"
            );

            return resultado.Values.FirstOrDefault();
        }

        private async Task AtualizarIMOAsync(int autonumBcg, string imo)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            string sql = @"UPDATE REDEX.dbo.tb_booking_carga
                            SET imo = @IMO
                          WHERE autonum_bcg = @AutonumBcg AND @IMO IS NOT NULL AND @IMO <> ''";

            await connection.ExecuteAsync(sql, new { AutonumBcg = autonumBcg, IMO = imo });
        }

        public async Task InserirCargaSoltaYardAsync(int autonumCS, int armazem, string yard)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            string sql = @"INSERT INTO SGIPA.DBO.TB_CARGA_SOLTA_YARD 
                               (Autonum_CS, Armazem, Yard, Origem, DATA, AUDIT_94, FL_FRENTE, FL_FUNDO, FL_LE, FL_LD)
                           VALUES 
                               (@AutonumCS, @Armazem, @Yard, 'R', GETDATE(), 0, 0, 0, 0, 0)";

            await connection.ExecuteAsync(sql, new
            {
                AutonumCS = autonumCS,
                Armazem = armazem,
                Yard = yard
            });
        }

        public async Task<bool> ValidarCargaTransferidaAsync(int talieId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            string sql = @"SELECT COUNT(1)
                            FROM REDEX.dbo.TB_PATIO_CS pcs
                            INNER JOIN REDEX.dbo.TB_TALIE_ITEM ti ON pcs.talie_descarga = ti.autonum_ti
                            WHERE ti.autonum_talie = @TalieId";

            int count = await connection.ExecuteScalarAsync<int>(sql, new { TalieId = talieId });

            if (count == 0)
            {
                throw new Exception("Falha no processo de fechamento. Carga não foi transferida para o estoque. Contate o TI assim que possível.");
            }

            return true;
        }

        public async Task<bool> FecharTalieAsync(int talieId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();

            string sql = @"
                          UPDATE REDEX.dbo.TB_TALIE 
                          SET FLAG_FECHADO = 1, 
                              DT_FECHAMENTO = GETDATE(),
                              TERMINO = GETDATE()
                          WHERE AUTONUM_TALIE = @TalieId";

            int rowsAffected = await connection.ExecuteAsync(sql, new { TalieId = talieId });

            return rowsAffected > 0; 
        }

        public async Task FinalizarReservaAsync(int booId)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync() as SqlConnection;
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // 🔹 Obtém QR (quantidade de carga da reserva)
                string sqlQR = @"
                                SELECT COALESCE(SUM(bcg.qtde), 0)
                                FROM REDEX.dbo.TB_BOOKING boo
                                INNER JOIN REDEX.dbo.TB_BOOKING_CARGA bcg ON boo.AUTONUM_BOO = bcg.AUTONUM_BOO
                                WHERE boo.AUTONUM_BOO = @Boo
                                AND bcg.FLAG_CS = 1";

                int qr = await connection.ExecuteScalarAsync<int>(sqlQR, new { Boo = booId }, transaction);

                // 🔹 Obtém QE (quantidade de entrada)
                string sqlQE = @"
                                SELECT COALESCE(SUM(pcs.QTDE_ENTRADA), 0)
                                FROM REDEX.dbo.TB_BOOKING boo
                                INNER JOIN REDEX.dbo.TB_BOOKING_CARGA bcg ON boo.AUTONUM_BOO = bcg.AUTONUM_BOO
                                INNER JOIN REDEX.dbo.TB_PATIO_CS pcs ON bcg.AUTONUM_BCG = pcs.AUTONUM_BCG
                                WHERE boo.AUTONUM_BOO = @Boo";

                int qe = await connection.ExecuteScalarAsync<int>(sqlQE, new { Boo = booId }, transaction);

                // 🔹 Se QE ≥ QR e QE > 0, finaliza a reserva
                if (qe >= qr && qe != 0)
                {
                    string sqlFinalizar = @"
                                           UPDATE REDEX.dbo.TB_BOOKING
                                           SET FLAG_FINALIZADO = 1
                                           WHERE AUTONUM_BOO = @Boo";

                    await connection.ExecuteAsync(sqlFinalizar, new { Boo = booId }, transaction);
                }

                // 🔹 Atualiza o status da reserva para 2
                string sqlStatusReserva = @"
                                           UPDATE REDEX.dbo.TB_BOOKING
                                           SET STATUS_RESERVA = 2
                                           WHERE AUTONUM_BOO = @Boo";

                await connection.ExecuteAsync(sqlStatusReserva, new { Boo = booId }, transaction);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Erro ao finalizar reserva: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Yard>> BuscarYard(string pesquisa)
        {
            using var connection = _connectionFactory.CreateConnection();
            string query = SqlQueries.BuscarYardPorTermo;

            var ret = await connection.QueryAsync<Yard>(query, new { term = $"%{pesquisa}%" });

            return ret;
        }


    }
}
