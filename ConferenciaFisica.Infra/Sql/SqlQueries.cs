namespace ConferenciaFisica.Infra.Sql
{
    public static class SqlQueries
    {
        public const string BuscarConferenciaPorIdContainer = @"SELECT DISTINCT 
                                                                    CONF.ID AS ID,
                                                                    CONF.TIPO_CONFERENCIA as Tipo,
                                                                    CONF.EMBALAGEM,
                                                                    CONF.BL as Lote,
                                                                    BL.VIAGEM,
                                                                    CONF.CNTR,
                                                                    CONF.INICIO,
                                                                    CONF.TERMINO,
                                                                    CONF.NOME_CLIENTE as NomeCliente,
                                                                    CONF.CPF_CLIENTE as CpfCliente,
                                                                    CONF.QTDE_AVARIADA as QuantidadeAvariada,
                                                                    CONF.OBS_AVARIA as ObservacaoAvaria,
                                                                    CONF.DIVERGENCIA_QTDE as QuantidadeDivergente,
                                                                    
                                                                    -- 🔄 Aqui convertemos 2 -> TRUE e qualquer outro valor -> FALSE
                                                                    CASE 
                                                                        WHEN CONF.DIVERGENCIA_QUALIFICACAO = 2 THEN CAST(1 AS BIT)
                                                                        ELSE CAST(0 AS BIT)
                                                                    END as DivergenciaQualificacao,
                                                                
                                                                    CONF.OBS_DIVERGENCIA as ObservacaoDivergencia,
                                                                    CONF.RETIRADA_AMOSTRA as RetiradaAmostra,
                                                                    CONF.CONFREMOTA as ConferenciaRemota,
                                                                    CONF.QTD_VOLUMES_DIVERGENTES as QuantidadeVolumesDivergentes,
                                                                    CONF.QTD_REPRESENTANTES as QuantidadeRepresentantes,
                                                                    CONF.QTD_AJUDANTES as QuantidadeAjudantes,
                                                                    CONF.QTD_OPERADORES as QuantidadeOperadores,
                                                                    CONF.MOVIMENTACAO as Movimentacao,
                                                                    CONF.DESUNITIZACAO as Desunitizacao,
                                                                    CONF.QTD_DOCUMENTOS as QuantidadeDocumentos,
                                                                
                                                                    CASE 
                                                                        WHEN ISNULL(CONF.BL, 0) <> 0 THEN 'CARGA SOLTA'
                                                                        WHEN ISNULL(CONF.CNTR, '') <> '' THEN 'CONTEINER'
                                                                        ELSE 'REDEX'
                                                                    END AS TipoCarga
                                                                
                                                                FROM dbo.TB_EFETIVACAO_CONF_FISICA AS CONF
                                                                LEFT JOIN dbo.TB_CNTR_BL BL ON CONF.CNTR = BL.ID_CONTEINER
                                                                WHERE BL.ID_CONTEINER = @idConteiner
                                                                ORDER BY CONF.ID DESC;

                                                                  ";
        public const string BuscarConferenciaPorId = @"SELECT
                                                        	DISTINCT 
                                                            CONF.ID AS ID,
                                                        	CONF.TIPO_CONFERENCIA as Tipo,
                                                        	CONF.EMBALAGEM,
                                                        	CONF.BL as Lote,
                                                        	BL.VIAGEM,
                                                        	CONF.CNTR,
                                                        	CONF.INICIO,
                                                        	CONF.TERMINO,
                                                        	CONF.NOME_CLIENTE as NomeCliente,
                                                        	CONF.CPF_CLIENTE as CpfCliente,
                                                        	CONF.QTDE_AVARIADA as QuantidadeAvariada,
                                                        	CONF.OBS_AVARIA as ObservacaoAvaria,
                                                        	CONF.DIVERGENCIA_QTDE as QuantidadeDivergente,
                                                        	-- 🔄 Aqui convertemos 2 -> TRUE e qualquer outro valor -> FALSE
                                                                                                                            CASE
                                                        		WHEN CONF.DIVERGENCIA_QUALIFICACAO = 2 THEN CAST(1 AS BIT)
                                                        		ELSE CAST(0 AS BIT)
                                                        	END as DivergenciaQualificacao,
                                                        	CONF.OBS_DIVERGENCIA as ObservacaoDivergencia,
                                                        	CONF.RETIRADA_AMOSTRA as RetiradaAmostra,
                                                        	CONF.CONFREMOTA as ConferenciaRemota,
                                                        	CONF.QTD_VOLUMES_DIVERGENTES as QuantidadeVolumesDivergentes,
                                                        	CONF.QTD_REPRESENTANTES as QuantidadeRepresentantes,
                                                        	CONF.QTD_AJUDANTES as QuantidadeAjudantes,
                                                        	CONF.QTD_OPERADORES as QuantidadeOperadores,
                                                        	CONF.MOVIMENTACAO as Movimentacao,
                                                        	CONF.DESUNITIZACAO as Desunitizacao,
                                                        	CONF.QTD_DOCUMENTOS as QuantidadeDocumentos,
                                                        	CASE
                                                        		WHEN ISNULL(CONF.BL,
                                                        		0) <> 0 THEN 'CARGA SOLTA'
                                                        		WHEN ISNULL(CONF.CNTR,
                                                        		'') <> '' THEN 'CONTEINER'
                                                        		ELSE 'REDEX'
                                                        	END AS TipoCarga
                                                        FROM
                                                        	dbo.TB_EFETIVACAO_CONF_FISICA AS CONF
                                                        LEFT JOIN dbo.TB_CNTR_BL BL ON
                                                        	CONF.CNTR = BL.ID_CONTEINER
                                                        WHERE
                                                        	CONF.ID = @id";
        public const string BUscarConferenciaPorLote = @"SELECT 
                                                            tecf.Id AS Id, 
                                                            tecf.BL AS NumeroBl, 
                                                            tecf.CNTR AS NumeroCntr, 
                                                            tecf.INICIO, 
                                                            tecf.TERMINO, 
                                                            tecf.NOME_CLIENTE, 
                                                            tecf.CPF_CLIENTE, 
                                                            tecf.QTDE_AVARIADA, 
                                                            tecf.OBS_AVARIA,
                                                            tecf.DIVERGENCIA_QTDE AS DivergenciaQuantidade, 
                                                            tecf.DIVERGENCIA_QUALIFICACAO, 
                                                            tecf.OBS_DIVERGENCIA, 
                                                            tecf.RETIRADA_AMOSTRA, 
                                                            CASE 
                                                                WHEN ISNULL(tecf.BL, 0) <> 0 THEN 'CARGA SOLTA'
                                                                WHEN ISNULL(tecf.CNTR, 0) <> 0 THEN 'CONTEINER'
                                                                ELSE 'REDEX'
                                                            END AS TipoCarga
                                                        FROM SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA tecf 
                                                        WHERE tecf.BL = @idLote";
        public const string BuscarConferenciaPorAgendamento = @"SELECT DISTINCT 
                                                                     '' AS NumeroBl,
                                                                     --C.AUTONUM AS CNTR,
                                                                     --B.NUMERO,
                                                                     B.VIAGEM as Viagem,
                                                                     D.EMBALAGEM as Embalagem,
                                                                     D.QUANTIDADE as Quantidade,
                                                                     C.ID_CONTEINER as Cntr,
                                                                     P.DT_PREVISTA as DataPrevista,
                                                                     T.DESCR AS MotivoAbertura,
                                                                     --G.QUANTIDADE AS QUANTIDADE_CNTR,
                                                                     G.EMBALAGEM AS EMBALAGEM_CNTR,
                                                                     'CONTEINER' AS TIPO_CARGA,
                                                                     C.LACRE_ORIGEM,
                                                                     C.LACRE2,
                                                                     C.LACRE3,
                                                                     C.LACRE4,
                                                                     C.LACRE_IPA
                                                                FROM dbo.TB_BL B  
                                                                LEFT JOIN dbo.TB_CARGA_SOLTA D ON D.BL = B.AUTONUM 
                                                                LEFT JOIN dbo.TB_AMR_CNTR_BL A ON A.BL = B.AUTONUM 
                                                                LEFT JOIN dbo.TB_CNTR_BL C ON A.CNTR = C.AUTONUM 
                                                                LEFT JOIN dbo.TB_CARGA_CNTR G ON G.BL = B.AUTONUM 
                                                                LEFT JOIN dbo.TB_AGENDAMENTO_POSICAO P ON P.CNTR = C.AUTONUM 
                                                                LEFT JOIN dbo.TB_AGENDA_POSICAO_MOTIVO M ON M.AUTONUM_AGENDA_POSICAO = P.AUTONUM
                                                                LEFT JOIN dbo.TB_MOTIVO_POSICAO T ON T.CODE = M.MOTIVO_POSICAO
                                                                WHERE P.DATA_CANCELAMENTO IS NULL 
                                                                AND C.ID_CONTEINER = @idConteiner --'AMFU315608-0'
                                                                ORDER BY P.DT_PREVISTA DESC";
        public const string BuscarConferenciaPorReserva = @"SELECT DISTINCT 
                                                                CONF.ID,
                                                                CONF.BL,
                                                                CONF.CNTR,
                                                                CONF.INICIO,
                                                                CONF.TERMINO,
                                                                CONF.NOME_CLIENTE,
                                                                CONF.CPF_CLIENTE,
                                                                CONF.QTDE_AVARIADA,
                                                                CONF.OBS_AVARIA,
                                                                ISNULL(CONF.DIVERGENCIA_QTDE, 0) AS DIVERGENCIA_QTDE,
                                                                CONF.DIVERGENCIA_QUALIFICACAO,
                                                                CONF.OBS_DIVERGENCIA,
                                                                CONF.RETIRADA_AMOSTRA,
                                                                tplc.ID_TIPO_LACRE as TipoLacre,
                                                                tplc.NUMERO_LACRE as Lacre,
                                                                CONF.CONFREMOTA,
                                                                CONF.QTD_VOLUMES_DIVERGENTES,
                                                                CONF.QTD_REPRESENTANTES,
                                                                CONF.QTD_AJUDANTES,
                                                                CONF.QTD_OPERADORES,
                                                                CONF.MOVIMENTACAO,
                                                                CONF.DESUNITIZACAO,
                                                                tplc.LACRE_FECHAMENTO as LacreFechamento, 
                                                                CONF.QTD_DOCUMENTOS,
                                                                A.AUTONUM_BOO,
                                                                A.REFERENCE AS NUMERO,
                                                                B.AUTONUM_EMB AS EMBALAGEM,
                                                                B.QTDE AS QUANTIDADE,
                                                                D.DT_PREVISTA,
                                                                T.DESCR AS MOTIVO,
                                                                'REDEX' AS TIPO_CARGA,
                                                                '' AS VIAGEM
                                                            FROM REDEX.dbo.TB_BOOKING A
                                                            LEFT JOIN REDEX.dbo.TB_BOOKING_CARGA B ON B.AUTONUM_BOO = A.AUTONUM_BOO
                                                            LEFT JOIN REDEX.dbo.TB_PATIO_CS C ON C.AUTONUM_BCG = B.AUTONUM_BCG
                                                            LEFT JOIN REDEX.dbo.TB_AGENDAMENTO_POSICAO D ON D.AUTONUM_PCS = C.AUTONUM_PCS
                                                            LEFT JOIN REDEX.dbo.TB_AGENDA_POSICAO_MOTIVO M ON M.AUTONUM_AGENDA_POSICAO = D.AUTONUM
                                                            LEFT JOIN SGIPA.dbo.TB_MOTIVO_POSICAO T ON T.CODE = M.MOTIVO
                                                            LEFT JOIN TB_EFETIVACAO_CONF_FISICA CONF ON CONF.AUTONUM_BOO = A.AUTONUM_BOO
                                                            LEFT JOIN SGIPA.dbo.TB_LACRES_CONFERENCIA tplc ON tplc.ID_CONFERENCIA = CONF.ID 
                                                            WHERE A.REFERENCE = @LotePesquisa --RESERVA '241ISZ1203091'";
        public const string CarregarConteinerAgendamento = @"SELECT 
															     ID_CONTEINER AS Display, 
															     ID_CONTEINER AS Autonum
															 FROM TB_AGENDAMENTO_POSICAO A
															 INNER JOIN TB_AGENDA_POSICAO_MOTIVO B ON A.AUTONUM = B.AUTONUM_AGENDA_POSICAO
															 INNER JOIN SGIPA.dbo.TB_CNTR_BL C ON A.CNTR = C.AUTONUM
															 WHERE CONVERT(VARCHAR, DT_PREVISTA, 103) = CONVERT(VARCHAR, GETDATE(), 103)
															 AND ID_STATUS_AGENDAMENTO = 0 
															 AND CNTR IS NOT NULL";
        public const string CarregarConteinerAgendamentoUnion = @"	UNION
																  SELECT 
																      ID_CONTEINER AS Display, 
																      ID_CONTEINER AS Autonum
																  FROM REDEX.dbo.TB_AGENDAMENTO_POSICAO A
																  INNER JOIN REDEX.dbo.TB_AGENDA_POSICAO_MOTIVO B ON A.AUTONUM = B.AUTONUM_AGENDA_POSICAO
																  INNER JOIN REDEX.dbo.TB_PATIO C ON A.AUTONUM_PATIO = C.AUTONUM_PATIO
																  WHERE ID_STATUS_AGENDAMENTO = 0 AND EF = 'F'";

        public const string InsertConferenciaFisica = @"INSERT
                                                        	INTO
                                                        	SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA (
                                                        	CNTR,
                                                        	BL,
                                                        	INICIO,
                                                        	CPF_CONFERENTE,
                                                        	NOME_CONFERENTE,
                                                        	CPF_CLIENTE,
                                                        	NOME_CLIENTE,
                                                        	DIVERGENCIA_QTDE,
                                                        	DIVERGENCIA_QUALIFICACAO,
                                                        	OBS_DIVERGENCIA,
                                                        	RETIRADA_AMOSTRA,
                                                        	--Lacre
                                                        	CONFREMOTA,
                                                        	OPERACAO,
                                                        	MODULO,
                                                        	QTD_VOLUMES_DIVERGENTES,
                                                        	QTD_REPRESENTANTES,
                                                        	QTD_AJUDANTES,
                                                        	QTD_OPERADORES,
                                                        	MOVIMENTACAO,
                                                        	DESUNITIZACAO,
                                                        	QTD_DOCUMENTOS
                                                        )
                                                        VALUES (
                                                        	@Cntr,
                                                            @Bl,@Inicio,@CpfConferente,@NomeConferente,@CpfCliente,
                                                        	@NomeCliente, @QuantidadeDivergente,@DivergenciaQualificacao,@ObservacaoDivergencia,
                                                        	@RetiradaAmostra,
                                                            @ConferenciaRemota,'I','C',
                                                            @QtdeVolumesDivergentes,
                                                            @QuantidadeRepresentantes,
                                                        	@QuantidadeAjudantes,
                                                            @QuantidadeOperadores,
                                                            @Movimentacao,@Desunitizacao,@QuantidadeDocumentos
                                                        )";
        public const string AtualizarConferencia = @"UPDATE TB_EFETIVACAO_CONF_FISICA
	                                                    SET TERMINO = GETDATE(), 
	                                                    CPF_CONFERENTE = @cpfConferente, 
	                                                    NOME_CONFERENTE = @nomeConferente,
	                                                    CPF_CLIENTE = @cpfCliente, 
	                                                    NOME_CLIENTE = @nomeCliente, 
	                                                    RETIRADA_AMOSTRA = @retiradaAmostra,
	                                                    --TIPO_AVARIA = @tipoAvaria,
	                                                    --QTDE_AVARIADA = @quantidadeAvariada,
	                                                    --OBS_AVARIA = @observacaoAvariada,
	                                                    DIVERGENCIA_QTDE = @quantidadeDivergente,
	                                                    DIVERGENCIA_QUALIFICACAO = @divergenciaQualificacao,
	                                                    OBS_DIVERGENCIA = @observacaoDivergencia,
	                                                    CONFREMOTA = @conferenciaRemota,
	                                                    EMBALAGEM = @embalagem,
	                                                    --PESO_AVARIADO = @pesoAvariado,
	                                                    QTD_REPRESENTANTES = @quantidadeRepresentantes,
	                                                    QTD_AJUDANTES = @quantidadeAjudantes,
	                                                    QTD_OPERADORES = @quantidadeOperadores,
	                                                    MOVIMENTACAO = @movimentacao,
	                                                    DESUNITIZACAO = @desunitizacao,
	                                                    QTD_DOCUMENTOS = @quantidadeDocumentos,
	                                                    QTD_VOLUMES_DIVERGENTES = @qtdeVolumesDivergentes, 
                                                        TIPO_CONFERENCIA = @tipo
	                                                 WHERE ID  = @ID";
        public const string CadastroAdicional = @"INSERT INTO TB_EFETIVACAO_CONF_FISICA_ADC (ID_CONFERENCIA,NOME, CPF, QUALIFICACAO, TIPO) VALUES (@idConferencia,@nome, @cpf, @qualificacao, @tipo)";
        public const string CarregarCadastrosAdicionais = @"SELECT ID, 
                                                                   ID_CONFERENCIA as IdConferencia, NOME, CPF, QUALIFICACAO, TIPO
                                                            FROM TB_EFETIVACAO_CONF_FISICA_ADC 
                                                            WHERE ID_CONFERENCIA = @idConferencia";
        public const string ExlcuirCadastroAdicional = @"DELETE FROM TB_EFETIVACAO_CONF_FISICA_ADC WHERE ID = @id";
        public const string CarregarTiposLacres = @"SELECT * FROM TB_TIPO_LACRE";
        public const string CarregarLacresConferencia = @"SELECT tlc.ID, 
                                                    	   tlc.ID_CONFERENCIA as IdConferencia, 
                                                    	   tlc.NUMERO_LACRE as Numero, 
                                                    	   tlc.ID_TIPO_LACRE as Tipo,
                                                    	   ttl.CODIGO + ' - ' + ttl.DESCRICAO as DescricaoTipo,
                                                    	   tlc.LACRE_FECHAMENTO as LacreFechamento 
                                                    FROM TB_LACRES_CONFERENCIA tlc 
                                                    INNER JOIN TB_TIPO_LACRE ttl ON ttl.ID = tlc.ID_TIPO_LACRE 
                                                    WHERE ID_CONFERENCIA = @idConferencia";
        public const string CadastrarLacreConferencia = @"INSERT INTO TB_LACRES_CONFERENCIA (ID_CONFERENCIA, NUMERO_LACRE, ID_TIPO_LACRE, LACRE_FECHAMENTO) 
                                                                                     VALUES (@idCOnferencia, @numero, @tipo, @lacreFechamento)";
        public const string AtualizarLacreConferencia = @"UPDATE TB_LACRES_CONFERENCIA SET 
                                                                                       ID_CONFERENCIA = @idConferencia, 
                                                                                       NUMERO_LACRE= @numero, 
                                                                                       ID_TIPO_LACRE=@tipo,
                                                                                       LACRE_FECHAMENTO= @lacreFechamento
                                                          WHERE ID = @id";
        public const string ExcluirLacreConferencia = @"DELETE FROM TB_LACRES_CONFERENCIA WHERE ID = @id ";

        public const string CarregarTiposDocumentos = @"SELECT * FROM TB_TIPO_DOCUMENTO_CONFERENCIA";
        public const string CarregarDocumentosConferencia = @"SELECT tdc.Id, 
                                                    	   tdc.ID_CONFERENCIA as IdConferencia,
                                                    	   tdc.NUMERO,
                                                    	   tdc.TIPO,
                                                    	   ttdc.DESCRICAO as TipoDescricao
                                                    	FROM TB_DOCUMENTOS_CONFERENCIA tdc 
                                                    	INNER JOIN TB_TIPO_DOCUMENTO_CONFERENCIA ttdc ON tdc.TIPO = ttdc.ID
                                                    	WHERE tdc.ID_CONFERENCIA = @idConferencia";
        public const string AtualizarDocumentosConferencia = @"UPDATE TB_DOCUMENTOS_CONFERENCIA 
                                                               		SET ID_CONFERENCIA = @idCOnferencia,
                                                               		    NUMERO = @numero,
                                                               		    TIPO = @tipo
                                                               WHERE ID = @id";
        public const string CadastrarDocumentosConferencia = @"INSERT INTO TB_DOCUMENTOS_CONFERENCIA (ID_CONFERENCIA, NUMERO, TIPO) VALUES(@idConferencia, @numero, @tipo)";
        public const string ExcluirDocumentosConferencia = @"DELETE FROM TB_DOCUMENTOS_CONFERENCIA WHERE ID = @id";

        public const string CarregarTiposAvarias = @"SELECT Id, Codigo, Descricao FROM TB_TIPOS_AVARIAS";
        public const string BuscarAvariasConferencia = @"SELECT tac.ID,
		                                                   tac.ID_CONFERENCIA as IdConferencia, 
		                                                   tac.QUANTIDADE_AVARIADA as QuantidadeAvariada, 
		                                                   tac.PESO_AVARIADO as PesoAvariado, 
		                                                   tac.ID_EMBALAGEM as IdEmbalagem,
		                                                   tac.CONTEINER as Conteiner,
		                                                   tac.OBSERVACAO as Observacao,
		                                                   --Tipos de Avarias
		                                                   tta.ID ,
		                                                   tta.CODIGO as Codigo,
		                                                   tta.DESCRICAO as Descricao
		                                                FROM TB_AVARIAS_CONFERENCIA tac 
		                                                INNER JOIN TB_AVARIA_CONFERENCIA_TIPO_AVARIA tacta on tac.ID = tacta.ID_AVARIA_CONFERENCIA 
		                                                INNER JOIN TB_TIPOS_AVARIAS tta ON tacta.ID_TIPO_AVARIA = tta.ID 
		                                                WHERE tac.ID_CONFERENCIA = @idConferencia";
        public const string CadastrarAvariaConferencia = @"INSERT INTO TB_AVARIAS_CONFERENCIA 
                                                          (ID_CONFERENCIA , QUANTIDADE_AVARIADA , PESO_AVARIADO , ID_EMBALAGEM , CONTEINER , OBSERVACAO) 
                                                          VALUES 
                                                          (@IdConferencia, @QuantidadeAvariada, @PesoAvariado, @IdEmbalagem, @Conteiner, @Observacao)";
        public const string AtualizarAvariaConferencia = @"UPDATE TB_AVARIAS_CONFERENCIA
	                                                        SET QUANTIDADE_AVARIADA = @quantidadeAvariada, 
	                                                        	PESO_AVARIADO = @pesoAvariado, 
	                                                        	ID_EMBALAGEM = @idEmbalagem,
	                                                        	CONTEINER = @conteiner,
	                                                        	OBSERVACAO = @observacao
	                                                        WHERE ID = @id";
        public const string CadastrarTiposAvariaConferencia = @"INSERT INTO TB_AVARIA_CONFERENCIA_TIPO_AVARIA (ID_AVARIA_CONFERENCIA, ID_TIPO_AVARIA)
                                                                VALUES (@IdAvariaConferencia, @IdTipoAvaria)";
        public const string ExcluirTiposAvariasConferencia = @"DELETE FROM TB_AVARIA_CONFERENCIA_TIPO_AVARIA WHERE ID_AVARIA_CONFERENCIA = @idAvariaConferencia";

        public const string CarregarTiposEmbalages = @"SELECT dte.AUTONUM_EMB as Id, 
                                                     	      dte.CODE as Codigo,
                                                     	      dte.DESCR as Descricao
                                                      FROM DTE_TB_EMBALAGENS dte";
        public const string FinalizarConferencia = @"UPDATE SGIPA.dbo.TB_EFETIVACAO_CONF_FISICA SET TERMINO = GETDATE() WHERE ID = @idConferencia";

        #region DESCARGA_EXPORTACAO
        public const string CarregarRegistro = @"SELECT
                                                    	--REGISTRO
                                                    	tr.AUTONUM_REG as Id,
                                                    	tr.placa as Placa,
                                                    	e.reference as Reserva,
                                                    	cexp.FANTASIA as Cliente,
                                                    	--TALIE
                                                        tt.AUTONUM_TALIE as Id,
                                                    	tt.INICIO as Inicio,
                                                    	tt.TERMINO as Termino,
                                                    	tt.CONFERENTE as Conferente,
                                                    	tt.EQUIPE as Equipe,
                                                    	tt.FORMA_OPERACAO as Operacao,
                                                    	tt.OBS as Observacao,
                                                    	--TALIE ITEM
                                                    	tti.AUTONUM_TI as Id,
                                                        tti.NF as NotaFiscal,
                                                        tti.REMONTE,
														tti.FUMIGACAO,
														tti.IMO As IMO1,
														tti.UNO As UNO1,
														tti.IMO2,
														tti.UNO2,
														tti.IMO3,
														tti.UNO3,
														tti.IMO4,
														tti.UNO4,
														tti.YARD,
														tti.FLAG_MADEIRA As Madeira,
														tti.FLAG_FRAGIL As Fragil,
														tti.AUTONUM_REGCS As RegistroCsId,
														tti.NF As NotaFiscal,
														tti.COMPRIMENTO,
														tti.LARGURA,
														tti.ALTURA,
														tti.PESO,
	                                                    tce.DESCRICAO_EMB as Embalagem,
														tce.SIGLA AS EmbalagemSigla,
														tce.AUTONUM_EMB As CodigoEmbalagem,
	                                                    --tti.QTDE_DISPONIVEL as QuantidadeNf,
                                                        (SELECT ISNULL(SUM(QUANTIDADE),0) FROM REDEX..TB_REGISTRO_CS WHERE AUTONUM_REGCS = trc.AUTONUM_REGCS) As QuantidadeNf,
	                                                    tti.QTDE_DESCARGA as QuantidadeDescarga
                                                    FROM
                                                    	REDEX.dbo.tb_gate_new a
                                                    INNER JOIN REDEX.dbo.tb_registro tr ON
                                                    	a.autonum = tr.autonum_gate
                                                    INNER JOIN REDEX.dbo.tb_booking e ON
                                                    	tr.autonum_boo = e.autonum_boo
                                                    INNER JOIN REDEX.dbo.tb_cad_parceiros ccli ON
                                                    	e.autonum_parceiro = ccli.autonum
                                                    INNER JOIN REDEX.dbo.tb_cad_parceiros cexp ON
                                                    	e.autonum_exportador = cexp.autonum
                                                    LEFT JOIN REDEX.dbo.tb_talie tt ON
                                                    	tr.autonum_reg = tt.autonum_reg
                                                    LEFT JOIN REDEX.dbo.TB_TALIE_ITEM tti ON
                                                    	tt.AUTONUM_TALIE = tti.AUTONUM_TALIE 
                                                    LEFT JOIN REDEX.dbo.TB_REGISTRO_CS trc ON
	                                                    trc.AUTONUM_REGCS = tti.AUTONUM_REGCS 
                                                    LEFT JOIN REDEX.dbo.TB_CAD_EMBALAGENS tce ON
	                                                    tti.AUTONUM_EMB = tce.AUTONUM_EMB 
                                                    WHERE
                                                    	tr.autonum_reg = @registro
                                                    	AND tr.TIPO_REGISTRO = 'E'";
        public const string AtualizarTalie = @" DECLARE @OutputTable TABLE (autonum_talie INT);
                                    
                                                UPDATE REDEX..tb_talie
                                                SET 
                                                    flag_descarga = 1,
                                                    flag_estufagem = 1,
                                                    flag_carregamento = 0,
                                                    crossdocking = 0,
                                                    conferente = @Conferente,
                                                    equipe = @Equipe,
                                                    forma_operacao = @Operacao,
                                                    termino =@termino
                                                OUTPUT INSERTED.autonum_talie INTO @OutputTable
                                                WHERE autonum_reg = @CodigoRegistro;
                                    
                                                SELECT autonum_talie FROM @OutputTable";
        public const string CriarTalie = @"INSERT INTO REDEX..TB_TALIE 
                                        (placa, 
                                        inicio, 
                                        flag_descarga, 
                                        flag_estufagem, 
                                        flag_carregamento, 
                                        crossdocking, 
                                        conferente, 
                                        equipe, 
                                        autonum_reg
                                        ) 
                                        VALUES 
                                        (
                                            @Placa,
                                            @Inicio, 
                                            1,--FLAG DESCARGA 
                                            0,--FLAG ESTUFAGEM
                                            0,--FLAG CARREGAMENTO 
                                            0,--CROSDOCKING 
                                            @Conferente, --ID 
                                            @Equipe, --ID
                                            --autonum_boo
                                            --forma_operacao
                                            --autonum_gate
                                            @CodigoRegistro --AutonumRegistro
                                        );
                                        
                                        SELECT CAST(SCOPE_IDENTITY() as int)";
        public const string ListaTalieItens = @"SELECT tti.AUTONUM_TI as Id,
                                            	   tti.NF,
                                            	   tti.QTDE_DESCARGA QtdDescarga,
                                                   (SELECT ISNULL(SUM(QUANTIDADE),0) FROM REDEX..TB_REGISTRO_CS WHERE AUTONUM_REGCS = trc.AUTONUM_REGCS) As QtdNf,
                                            	   tti.AUTONUM_EMB as CodigoEmbalagem,
                                            	   tce.DESCRICAO_EMB as Embalagem,
                                            	   tti.COMPRIMENTO ,
                                            	   tti.LARGURA ,
                                            	   tti.ALTURA ,
                                            	   tti.PESO ,
                                            	   tti.IMO ,
                                            	   tti.IMO2 ,
                                            	   tti.IMO3 ,
                                            	   tti.IMO4 ,
                                            	   tti.IMO5 ,
                                            	   tti.UNO ,
                                            	   tti.UNO2 ,
                                            	   tti.UNO3 ,
                                            	   tti.UNO4 ,
                                            	   tti.UNO5 ,
                                            	   tti.REMONTE ,
                                            	   tti.FUMIGACAO ,
                                            	   tti.FLAG_MADEIRA as FlagMadeira,
                                            	   tti.FLAG_FRAGIL as FlagFragil,
                                                   tti.OBS as Observacao
                                            FROM REDEX.dbo.TB_TALIE_ITEM tti 
                                            INNER JOIN REDEX.dbo.TB_CAD_EMBALAGENS tce ON tti.AUTONUM_EMB = tce.AUTONUM_EMB 
                                            INNER JOIN REDEX.dbo.TB_REGISTRO_CS trc ON
	                                                    trc.AUTONUM_REGCS = tti.AUTONUM_REGCS 
                                            WHERE tti.AUTONUM_TALIE = @TalieId";

        public const string Descarga = @"SELECT 
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
        public const string CadastrarAvariaCs = @"INSERT
                                                	INTO
                                                	REDEX.dbo.TB_AVARIAS_CS (
                                                	[LOCAL],
                                                	TIPO,
                                                	OBS_TERMINAL,
                                                    AUTONUM_AVCS,
                                                	QTDE_AVARIADA,
                                                	PESO_AVARIADO,
                                                	AUTONUM_TALIE,
                                                	DATA_AVARIA,
                                                	FLAG_DIVERGENCIA)
                                                VALUES (@local,
                                                @tipo,
                                                @observacao,
                                                @autonumAvcs,
                                                @quantidadeAvariada,
                                                @pesoAvariado,
                                                @talieId,
                                                GETDATE(),
                                                @flagDivergencia)";
        public const string ExcluirAvarias = @"DELETE FROM REDEX.dbo.TB_AVARIAS_CS WHERE AUTONUM_TALIE = @talieId";

        public const string AtualizarTalieItem = @"UPDATE REDEX.dbo.TB_TALIE_ITEM  SET QTDE_DESCARGA = @QtdDescarga, 
                                                   										   AUTONUM_EMB = @IdEmbalagem, 
                                                   										   COMPRIMENTO = @Comprimento, 
                                                   										   ALTURA = @Altura, 
                                                   										   LARGURA = @Largura, 
                                                   										   PESO =@Peso,
                                                   										   IMO = @Imo,
                                                   										   IMO2 = @Imo2,
                                                   										   IMO3 = @Imo3,
                                                   										   IMO4 = @Imo4,
                                                   										   IMO5 = @Imo5,
                                                   										   UNO = @Uno,
                                                   										   UNO2 = @Uno2,
                                                   										   UNO3 = @Uno3,
                                                   										   UNO4 = @Uno4,
                                                   										   UNO5 = @Uno5,
                                                                                           OBS = @Observacao,
                                                                                           FLAG_MADEIRA = @Madeira,
                                                                                           FLAG_FRAGIL = @Fragil,
                                                                                           REMONTE = @Remonte
                                                                                           
                                                   WHERE AUTONUM_TI = @TalieItemId";
        public const string BuscarTaliItemPorId = @"SELECT
                        TI.AUTONUM_TI As Id,
                        TI.AUTONUM_TALIE As TalieId,
                        TI.AUTONUM_NF As NotaFiscalId,
                        (SELECT ISNULL(SUM(QTDE_DESCARGA),0) FROM REDEX..TB_TALIE_ITEM WHERE AUTONUM_REGCS = TI.AUTONUM_REGCS AND AUTONUM_TI = @id) As QtdDescarga,
                        (SELECT ISNULL(SUM(QUANTIDADE),0) FROM REDEX..TB_REGISTRO_CS WHERE AUTONUM_REGCS = TI.AUTONUM_REGCS) As Quantidade,
                        TI.REMONTE,
                        TI.FUMIGACAO,
                        TI.IMO As IMO1,
                        TI.UNO As UNO1,
                        TI.IMO2,
                        TI.UNO2,
                        TI.IMO3,
                        TI.UNO3,
                        TI.IMO4,
                        TI.UNO4,
                        TI.YARD,
                        TI.FLAG_MADEIRA As Madeira,
                        TI.FLAG_FRAGIL As Fragil,
                        TI.AUTONUM_REGCS As RegistroCsId,
                        TI.NF As NotaFiscal,
                        TI.COMPRIMENTO,
                        TI.LARGURA,
                        TI.ALTURA,
                        TI.PESO,
                        E.SIGLA,
                        E.SIGLA AS EmbalagemSigla,
                        E.AUTONUM_EMB As CodigoEmbalagem,
                        E.DESCRICAO_EMB AS EMBALAGEM,
                        TI.YARD
                    FROM
                        REDEX..TB_TALIE_ITEM TI
                    LEFT JOIN
                        REDEX..TB_NOTAS_FISCAIS NF ON TI.AUTONUM_NF = NF.AUTONUM_NF
                    LEFT JOIN
                        REDEX..TB_CAD_EMBALAGENS E ON TI.AUTONUM_EMB = E.AUTONUM_EMB
                    WHERE
                        TI.AUTONUM_TI = @Id";
        public const string ExcluirTalieItem = @"DELETE FROM REDEX.dbo.TB_TALIE_ITEM WHERE AUTONUM_TI = @id";
        public const string GravarObservacao = @"UPDATE REDEX.dbo.TB_TALIE SET OBS = @Observacao WHERE AUTONUM_TALIE = @talieId";

        public const string ListarArmazensPorPatio = @"SELECT tai.AUTONUM as Id,
		                                                  tai.DESCR as Descricao
		                                               FROM TB_ARMAZENS_IPA tai 
		                                               WHERE tai.DT_SAIDA is NULL AND tai.FLAG_HISTORICO =0 and tai.PATIO = @patio";

        public const string GravarMarcante = @"UPDATE REDEX.dbo.TB_MARCANTES_RDX 
	                                           SET DT_ASSOCIACAO = GETDATE(), 
	                                           	ARMAZEM = @armazem, 
	                                           	--PLACA_C = @placa,
	                                           	AUTONUM_TALIE = @talieId,
	                                           	AUTONUM_TI = @talieItemId
	                                           WHERE AUTONUM_REG = @idRegistro";


        public const string ListarTiposFoto = @"SELECT ID as Id, Codigo, Descricao FROM REDEX.dbo.TB_TIPOS_FOTO;";

        public const string ListarTiposProcesso = @"SELECT ID as Id, Descricao FROM REDEX.dbo.TB_TIPOS_PROCESSO;";

        public const string ListarTiposProcessoFoto = @"SELECT * FROM REDEX.dbo.TB_PROCESSO_FOTO;";

        public const string GetTiposProcessoById = @"SELECT ID as Id, Descricao FROM REDEX.dbo.TB_TIPOS_PROCESSO  WHERE ID = @Id; ";

        public const string GetTiposProcessoFotoById = @"SELECT ID as Id, * FROM REDEX.dbo.TB_PROCESSO_FOTO  WHERE ID = @Id; ";

        public const string GetTiposProcessoFotoByProcessoId = @"SELECT 
                                                                    PF.ID AS Id,
                                                                    TF.ID AS TipoFotoID,          
                                                                    TF.Descricao,
                                                                    PF.TipoProcessoID,
                                                                    PF.Ativo
                                                                FROM 
                                                                    REDEX.dbo.TB_PROCESSO_FOTO PF
                                                                INNER JOIN 
                                                                    REDEX.dbo.TB_TIPOS_FOTO TF ON PF.TipoFotoID = TF.ID
                                                                INNER JOIN 
                                                                    REDEX.dbo.TB_TIPOS_PROCESSO TP ON PF.TipoProcessoID = TP.ID 
                                                                WHERE 
                                                                    TP.Descricao = @ProcessDescription
                                                                    AND PF.Ativo = 1; ";

        public const string UpdateTiposProcesso = @"UPDATE REDEX.dbo.TB_TIPOS_PROCESSO 
                                                                        SET Descricao = @Descricao
                                                                      WHERE ID = @Id;";

        public const string UpdateTiposProcessoFoto = @"UPDATE REDEX.dbo.TB_PROCESSO_FOTO 
                                                                        SET Ativo = @Ativo,
                                                                             TipoProcessoID = @TipoProcessoID,
                                                                             TipoFotoID = @TipoFotoID
                                                                      WHERE ID = @Id;";

        public const string ListarProcessosPorTalie = @"SELECT 
                                                            FP.ID AS Id, 
                                                            FP.ID_TIPO_PROCESSO AS IdTipoProcesso, 
                                                            FP.ID_TALIE AS IdTalie,
                                                            FP.IMAGEM_PATH AS ImagemPath,
                                                            FP.OBSERVACAO AS Observacao,
                                                            FP.DESCRICAO AS Descricao,
                                                            TP.Descricao AS DescricaoTipoProcesso
                                                        FROM 
                                                            REDEX.dbo.TB_FOTO_PROCESSO FP
                                                        JOIN 
                                                            REDEX.dbo.TB_TIPOS_FOTO TP ON FP.ID_TIPO_FOTO = TP.ID
                                                        WHERE 
                                                            FP.ID_TALIE = @talieId;";

        public const string ListarProcessosPorContainer = @"SELECT 
                                                            FP.ID AS Id, 
                                                            FP.ID_TIPO_PROCESSO AS IdTipoProcesso, 
                                                            FP.ID_TALIE AS IdTalie,
                                                            FP.IMAGEM_PATH AS ImagemPath,
                                                            FP.OBSERVACAO AS Observacao,
                                                            FP.DESCRICAO AS Descricao,
                                                            FP.ID_CONTAINER AS IdContainer,
                                                            TP.Descricao AS DescricaoTipoProcesso
                                                        FROM 
                                                            REDEX.dbo.TB_FOTO_PROCESSO FP
                                                        JOIN 
                                                            REDEX.dbo.TB_TIPOS_FOTO TP ON FP.ID_TIPO_FOTO = TP.ID
                                                        WHERE 
                                                            FP.ID_CONTAINER = @container;";

        public const string InsertTipoProcesso = @"INSERT INTO
                                                        	REDEX.dbo.TB_TIPOS_FOTO (Codigo, Descricao)
                                                        VALUES (
                                                        	@Codigo,
                                                            @Descricao
                                                        )";

        public const string InsertTiposProcesso = @"INSERT INTO
                                                        	REDEX.dbo.TB_TIPOS_PROCESSO (Descricao)
                                                        VALUES (
                                                            @Descricao
                                                        )";

        public const string InsertTiposProcessoFoto = @"INSERT INTO
                                                        	REDEX.dbo.TB_PROCESSO_FOTO (TipoProcessoID, TipoFotoID, Ativo)
                                                        VALUES (
                                                            @TipoProcessoID,
                                                            @TipoFotoID,
                                                            @Ativo
                                                        )";

        public const string DeleteTipoFoto = @"DELETE FROM REDEX.dbo.TB_TIPOS_FOTO WHERE ID = @id";
        public const string DeleteTipoProcesso = @"DELETE FROM REDEX.dbo.TB_TIPOS_PROCESSO WHERE ID = @id";
        public const string DeleteTipoProcessoFoto = @"DELETE FROM REDEX.dbo.TB_PROCESSO_FOTO WHERE ID = @id";

        public const string DeleteProcesso = @"DELETE FROM REDEX.dbo.TB_FOTO_PROCESSO WHERE ID = @id";

        public const string InsertProcesso = @"INSERT INTO
                                                        	REDEX.dbo.TB_FOTO_PROCESSO (
                                                                ID_TIPO_FOTO,        
                                                                ID_TIPO_PROCESSO,    
                                                                ID_TALIE,            
                                                                ID_CONTAINER,        
                                                                IMAGEM_PATH,         
                                                                DESCRICAO,           
                                                                OBSERVACAO           
                                                            )
                                                        VALUES (
                                                        	@IdTipoFoto,
                                                        	@IdProcesso,
                                                        	@IdTalie,
                                                        	@IdContainer,
                                                        	@ImagemPath,
                                                        	@Descricao,
                                                        	@Observacao
                                                        )";

        public const string UpdatesProcessoDescricaoAndObservacao = @"UPDATE REDEX.dbo.TB_FOTO_PROCESSO 
                                                                        SET DESCRICAO = @Descricao, OBSERVACAO = @Observacao
                                                                      WHERE ID = @Id;";

        public const string UpdatesTipoFoto = @"UPDATE REDEX.dbo.TB_TIPOS_FOTO 
                                                                        SET Codigo = @Codigo, Descricao = @Descricao
                                                                      WHERE ID = @Id;";

        public const string CarregarMarcantesTalieItem = @"SELECT tmr.AUTONUM as Id,
	                                                          tmr.DT_IMPRESSAO as DataImpressao,
	                                                          tmr.DT_ASSOCIACAO as DataAssociacao,
	                                                          tmr.VOLUMES as Quantidade ,
	                                                          tmr.AUTONUM_TALIE as TalieId,
	                                                          tmr.AUTONUM_TI as TalieItemId,
	                                                          tmr.STR_CODE128 as Numero,
	                                                          tmr.AUTONUM_CS_YARD as Local
	                                                       FROM REDEX.dbo.TB_MARCANTES_RDX tmr 
	                                                       WHERE tmr.AUTONUM_TI = @talieItemId";
        public const string ExcluirMarcanteTalieItem = @"UPDATE REDEX.dbo.TB_MARCANTES_RDX 
	                                                    SET DT_ASSOCIACAO = null, 
	                                                    	ARMAZEM = 0, 
	                                                    	--PLACA_C = @placa,
	                                                    	AUTONUM_TALIE = 0,
	                                                    	AUTONUM_TI = 0
	                                                    WHERE AUTONUM = @id";

        public const string BuscarRegistroDescarga = @"SELECT
                                                        	--TALIE
                                                        	a.autonum_talie as Id,
                                                        	a.termino as DataTermino,
                                                        	a.autonum_boo as Booking,
                                                        	a.autonum_gate as Gate,
                                                        	a.forma_operacao as Operacao,
                                                        	--TALIE ITEM
                                                        	b.autonum_ti as Id,
                                                        	b.qtde_descarga AS quantidade,
                                                        	b.autonum_emb as Embalagem,
                                                        	b.autonum_pro as Produto,
                                                        	b.marca as Marca,
                                                        	b.qtde_estufagem as QuantidadeEstufagem,
                                                        	b.comprimento as Comprimento,
                                                        	b.largura as Largura,
                                                        	b.altura as Altura,
                                                        	b.peso as Peso,
                                                        	b.yard as Yard,
                                                        	b.armazem as Armazem,
                                                        	--'' CodProduto AS codproduto,
                                                        	b.imo as IMO,
                                                        	b.imo2 as IMO2,
                                                        	b.imo3 as IMO3,
                                                        	b.imo4 as IMO4,
                                                        	b.uno as UNO,
                                                        	b.uno2 as UNO2,
                                                        	b.uno3 as UNO3,
                                                        	b.uno4 as UNO4,
                                                        	b.autonum_regcs as RegistroCargaSolta,
                                                        	b.cod_ean as CodigoEan,
                                                        	boo.fcl_lcl as FclLcl,
                                                        	c.autonum_nf as NotaFiscal,
                                                        	d.autonum_emb AS EmbalagemReserva,
                                                        	d.autonum_bcg as IdBookingCarga
                                                        FROM
                                                        	REDEX.dbo.tb_talie a
                                                        INNER JOIN REDEX.dbo.tb_talie_item b ON
                                                        	a.autonum_talie = b.autonum_talie
                                                        LEFT JOIN REDEX.dbo.tb_notas_fiscais c ON
                                                        	b.autonum_nf = c.autonum_nf
                                                        INNER JOIN REDEX.dbo.tb_registro_cs e ON
                                                        	e.autonum_regcs = b.autonum_regcs
                                                        INNER JOIN REDEX.dbo.tb_booking_carga d ON
                                                        	d.autonum_bcg = e.autonum_bcg
                                                        INNER JOIN REDEX.dbo.tb_booking boo ON
                                                        	d.autonum_boo = boo.autonum_boo
                                                        WHERE
                                                        	a.autonum_talie = @talieId";
        public const string InsertIntoTbPatioCs = @"INSERT INTO REDEX.dbo.TB_PATIO_CS (
                                                        autonum_pcs, --id da tabela
                                                        AUTONUM_BCG, 
                                                        QTDE_ENTRADA, 
                                                        AUTONUM_EMB, 
                                                        autonum_pro, 
                                                        MARCA, 
                                                        VOLUME_DECLARADO, 
                                                        COMPRIMENTO, 
                                                        LARGURA, 
                                                        ALTURA, 
                                                        BRUTO, 
                                                        qtde_unit, 
                                                        DT_PRIM_ENTRADA, 
                                                        FLAG_HISTORICO,
                                                        AUTONUM_REGCS, 
                                                        AUTONUM_NF, 
                                                        talie_descarga, 
                                                        QTDE_ESTUFAGEM, 
                                                        YARD, 
                                                        ARMAZEM, 
                                                        AUTONUM_PATIOS, 
                                                        PATIO, 
                                                        imo, uno, imo2, uno2, imo3, uno3, imo4, uno4, codproduto, cod_ean
                                                    ) VALUES (
                                                        @Id, @AutonumBcg, @QuantidadeEntrada, @AutonumEmb, @AutonumPro, @Marca, 
                                                        @VolumeDeclarado, @Comprimento, @Largura, @Altura, @Bruto, @QtdeUnit,
                                                        @DataRegistro, 0, @AutonumRegcs, @AutonumNf, @AutonumTi, @QtdeEstufagem,
                                                        @Yard, @Armazem, @AutonumPatios, @Patio, @Imo, @Uno, @Imo2, @Uno2, @Imo3, 
                                                        @Uno3, @Imo4, @Uno4, @CodProduto, @CodEan
                                                    )";

        #endregion DESCARGA_EXPORTACAO
    }
}
