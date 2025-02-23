namespace ConferenciaFisica.Infra.Sql
{
    public static class SqlQueries
    {
        public const string BuscarConferencia = @"SELECT
												     ID AS Id,
												     BL AS NumeroBl,
												     CNTR AS NumeroCntr,
												     INICIO as Inicio,
												     TERMINO as Termino,
												     NOME_CLIENTE as NomeCliente,
												     CPF_CLIENTE as CpfCliente,
												     QTDE_AVARIADA as QuantidadeAvariada,
												     OBS_AVARIA as ObservacaoAvaria,
												     DIVERGENCIA_QTDE AS DivergenciaQuantidade,
												     DIVERGENCIA_QUALIFICACAO as DivergenciaQualificacao,
												     OBS_DIVERGENCIA as ObservacaoDivergencia,
												     RETIRADA_AMOSTRA as RetiradaAmostra,
												     CASE
												     	WHEN ISNULL(BL,
												     	0) <> 0 THEN 'CARGA SOLTA'
												     	WHEN ISNULL(CNTR,
												     	0) <> 0 THEN 'CONTEINER'
												     	ELSE 'REDEX'
												     END AS TipoCarga
													FROM
														Sgipa.dbo.TB_EFETIVACAO_CONF_FISICA
                                                   WHERE CNTR = @idConteiner;
                                                   ";
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
    }
}
