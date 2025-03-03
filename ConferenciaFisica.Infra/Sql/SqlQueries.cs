namespace ConferenciaFisica.Infra.Sql
{
    public static class SqlQueries
    {
        public const string BuscarConferenciaPorIdContainer = @"SELECT DISTINCT 
                                                        CONF.ID AS ID,
                                                        CONF.BL as Lote,
                                                        BL.VIAGEM ,
                                                        CONF.CNTR,
                                                        CONF.INICIO,
                                                        CONF.TERMINO,
                                                        CONF.NOME_CLIENTE as NomeCliente,
                                                        CONF.CPF_CLIENTE as CpfCliente,
                                                        CONF.QTDE_AVARIADA as QuantidadeAvariada,
                                                        CONF.OBS_AVARIA as ObservacaoAvaria,
                                                        CONF.DIVERGENCIA_QTDE as QuantidadeDivergente,
                                                        CONF.DIVERGENCIA_QUALIFICACAO as DivergenciaQualificacao,
                                                        CONF.OBS_DIVERGENCIA as ObservacaoDivergencia,
                                                        CONF.RETIRADA_AMOSTRA as RetiradaAmostra,
                                                        --LACRES
                                                        CONF.CONFREMOTA as ConferenciaRemota,
                                                        CONF.QTD_VOLUMES_DIVERGENTES as QuantidadeVolumesDivergentes,
                                                        CONF.QTD_REPRESENTANTES as QuantidadeRepresentantes,
                                                        CONF.QTD_AJUDANTES as QuantidadeAjudantes,
                                                        CONF.QTD_OPERADORES as QuantidadeOperadores,
                                                        CONF.MOVIMENTACAO as Movimentacao,
                                                        CONF.DESUNITIZACAO as Desunitizacao,
                                                        --Lacrefechamento
                                                        CONF.QTD_DOCUMENTOS as QuantidadeDocumentos,
                                                        CASE 
                                                            WHEN ISNULL(CONF.BL, 0) <> 0 THEN 'CARGA SOLTA'
                                                            WHEN ISNULL(CONF.CNTR, '') <> '' THEN 'CONTEINER'
                                                            ELSE 'REDEX'
                                                        END AS TipoCarga
                                                    FROM dbo.TB_EFETIVACAO_CONF_FISICA AS CONF
                                                    LEFT JOIN dbo.TB_CNTR_BL BL ON CONF.CNTR = BL.ID_CONTEINER
                                                    WHERE BL.ID_CONTEINER = @idConteiner --'AMFU315608-0'
                                                    ORDER BY CONF.ID DESC
                                                   ";
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
                                                        	@NomeCliente, @QtdeDivergente,@DivergenciaQualificacao,@ObservacaoDivergencia,
                                                        	@RetiradaAmostra,
                                                            @ConferenciaRemota,'I','C',
                                                            @QtdeVolumesDivergentes,
                                                            @QtdeRepresentantes,
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
	                                                    QTD_VOLUMES_DIVERGENTES = @qtdeVolumesDivergentes
	                                                 WHERE ID  = @ID";
        public const string CadastroAdicional = @"INSERT INTO TB_EFETIVACAO_CONF_FISICA_ADC (ID_CONFERENCIA,NOME, CPF, QUALIFICACAO, TIPO) VALUES (@idConferencia,@nome, @cpf, @qualificacao, @tipo)";
        public const string CarregarCadastrosAdicionais = @"SELECT ID, 
                                                                   ID_CONFERENCIA as IdConferencia, NOME, CPF, QUALIFICACAO, TIPO
                                                            FROM TB_EFETIVACAO_CONF_FISICA_ADC 
                                                            WHERE ID_CONFERENCIA = @idConferencia";
    
    }
}
