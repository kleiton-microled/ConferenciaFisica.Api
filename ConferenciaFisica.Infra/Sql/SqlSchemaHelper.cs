using ConferenciaFisica.Application.Interfaces;

namespace ConferenciaFisica.Infra.Sql
{
    public static class SqlSchemaHelper
    {
        /// <summary>
        /// Substitui os schemas hardcoded nas consultas SQL pelo schema dinâmico
        /// </summary>
        public static string ReplaceSchema(string sql, ISchemaService schemaService)
        {
            if (string.IsNullOrEmpty(sql) || schemaService == null)
                return sql;

            var currentSchema = schemaService.GetCurrentSchema();
            
            // Substitui schemas hardcoded pelas tabelas afetadas
            var replacements = new Dictionary<string, string>
            {
                { "dbo.TB_EFETIVACAO_CONF_FISICA", $"{currentSchema}.dbo.TB_EFETIVACAO_CONF_FISICA" },
                { "dbo.TB_TIPO_LACRE", $"{currentSchema}.dbo.TB_TIPO_LACRE" },
                { "dbo.TB_LACRES_CONFERENCIA", $"{currentSchema}.dbo.TB_LACRES_CONFERENCIA" },
                { "dbo.TB_DOCUMENTOS_CONFERENCIA", $"{currentSchema}.dbo.TB_DOCUMENTOS_CONFERENCIA" },
                { "dbo.TB_TIPO_DOCUMENTO_CONFERENCIA", $"{currentSchema}.dbo.TB_TIPO_DOCUMENTO_CONFERENCIA" },
                { "dbo.TB_TIPOS_AVARIAS", $"{currentSchema}.dbo.TB_TIPOS_AVARIAS" },
                { "dbo.TB_AVARIAS_CONFERENCIA", $"{currentSchema}.dbo.TB_AVARIAS_CONFERENCIA" },
                { "dbo.TB_AVARIA_CONFERENCIA_TIPO_AVARIA", $"{currentSchema}.dbo.TB_AVARIA_CONFERENCIA_TIPO_AVARIA" },
                { "dbo.TB_EFETIVACAO_CONF_FISICA_ADC", $"{currentSchema}.dbo.TB_EFETIVACAO_CONF_FISICA_ADC" },
                { "dbo.VW_CONF_FISICA_SELECAO_CNTR", $"{currentSchema}.dbo.VW_CONF_FISICA_SELECAO_CNTR" },
                
                // Também substitui sem dbo. para casos onde não especificado
                { "TB_EFETIVACAO_CONF_FISICA", $"{currentSchema}.dbo.TB_EFETIVACAO_CONF_FISICA" },
                { "TB_TIPO_LACRE", $"{currentSchema}.dbo.TB_TIPO_LACRE" },
                { "TB_LACRES_CONFERENCIA", $"{currentSchema}.dbo.TB_LACRES_CONFERENCIA" },
                { "TB_DOCUMENTOS_CONFERENCIA", $"{currentSchema}.dbo.TB_DOCUMENTOS_CONFERENCIA" },
                { "TB_TIPO_DOCUMENTO_CONFERENCIA", $"{currentSchema}.dbo.TB_TIPO_DOCUMENTO_CONFERENCIA" },
                { "TB_TIPOS_AVARIAS", $"{currentSchema}.dbo.TB_TIPOS_AVARIAS" },
                { "TB_AVARIAS_CONFERENCIA", $"{currentSchema}.dbo.TB_AVARIAS_CONFERENCIA" },
                { "TB_AVARIA_CONFERENCIA_TIPO_AVARIA", $"{currentSchema}.dbo.TB_AVARIA_CONFERENCIA_TIPO_AVARIA" },
                { "TB_EFETIVACAO_CONF_FISICA_ADC", $"{currentSchema}.dbo.TB_EFETIVACAO_CONF_FISICA_ADC" },
                { "VW_CONF_FISICA_SELECAO_CNTR", $"{currentSchema}.dbo.VW_CONF_FISICA_SELECAO_CNTR" }
            };

            var result = sql;
            foreach (var replacement in replacements)
            {
                result = result.Replace(replacement.Key, replacement.Value);
            }

            return result;
        }

        /// <summary>
        /// Obtém o nome da tabela com schema dinâmico
        /// </summary>
        public static string GetTableName(string tableName, ISchemaService schemaService)
        {
            return schemaService.GetTableName(tableName);
        }

        /// <summary>
        /// Constrói a query de inserção de conferência física baseada no schema
        /// </summary>
        public static string BuildInsertConferenciaFisicaQuery(ISchemaService schemaService)
        {
            var isRedex = schemaService.GetCurrentSchema() == "REDEX";
            var tableName = schemaService.GetTableName("TB_EFETIVACAO_CONF_FISICA");

            if (isRedex)
            {
                // Para REDEX, NÃO inclui a coluna BL
                return $@"INSERT INTO {tableName} (
                    TIPO_CONFERENCIA,
                    AUTONUM_PATIO,
                    INICIO,
                    CPF_CONFERENTE,
                    NOME_CONFERENTE,
                    TELEFONE_CONFERENTE,
                    CPF_CLIENTE,
                    NOME_CLIENTE,
                    DIVERGENCIA_QTDE,
                    DIVERGENCIA_QUALIFICACAO,
                    OBS_DIVERGENCIA,
                    RETIRADA_AMOSTRA,
                    CONFREMOTA,
                    OPERACAO,
                    MODULO,
                    QTD_VOLUMES_DIVERGENTES,
                    QTD_REPRESENTANTES,
                    QTD_AJUDANTES,
                    QTD_OPERADORES,
                    MOVIMENTACAO,
                    DESUNITIZACAO,
                    QTD_DOCUMENTOS,
                    AUTONUM_AGENDA_POSICAO
                )
                VALUES (
                    @tipoConferencia,
                    @Cntr,
                    GETDATE(),
                    @CpfConferente,
                    @NomeConferente,
                    @telefoneConferente,
                    @CpfCliente,
                    @NomeCliente,
                    @QuantidadeDivergente,
                    @DivergenciaQualificacao,
                    @ObservacaoDivergencias,
                    @RetiradaAmostra,
                    @ConferenciaRemota,
                    'I',
                    'C',
                    @QuantidadeVolumesDivergentes,
                    @QuantidadeRepresentantes,
                    @QuantidadeAjudantes,
                    @QuantidadeOperadores,
                    @Movimentacao,
                    @Desunitizacao,
                    @QuantidadeDocumentos,
                    @autonumAgendaPosicao
                )";
            }
            else
            {
                // Para outros schemas, inclui a coluna BL
                return $@"INSERT INTO {tableName} (
                    TIPO_CONFERENCIA,
                    CNTR,
                    BL,
                    INICIO,
                    CPF_CONFERENTE,
                    NOME_CONFERENTE,
                    TELEFONE_CONFERENTE,
                    CPF_CLIENTE,
                    NOME_CLIENTE,
                    DIVERGENCIA_QTDE,
                    DIVERGENCIA_QUALIFICACAO,
                    OBS_DIVERGENCIA,
                    RETIRADA_AMOSTRA,
                    CONFREMOTA,
                    OPERACAO,
                    MODULO,
                    QTD_VOLUMES_DIVERGENTES,
                    QTD_REPRESENTANTES,
                    QTD_AJUDANTES,
                    QTD_OPERADORES,
                    MOVIMENTACAO,
                    DESUNITIZACAO,
                    QTD_DOCUMENTOS,
                    AUTONUM_AGENDA_POSICAO
                )
                VALUES (
                    @tipoConferencia,
                    @Cntr,
                    @Bl,
                    GETDATE(),
                    @CpfConferente,
                    @NomeConferente,
                    @telefoneConferente,
                    @CpfCliente,
                    @NomeCliente,
                    @QuantidadeDivergente,
                    @DivergenciaQualificacao,
                    @ObservacaoDivergencias,
                    @RetiradaAmostra,
                    @ConferenciaRemota,
                    'I',
                    'C',
                    @QuantidadeVolumesDivergentes,
                    @QuantidadeRepresentantes,
                    @QuantidadeAjudantes,
                    @QuantidadeOperadores,
                    @Movimentacao,
                    @Desunitizacao,
                    @QuantidadeDocumentos,
                    @autonumAgendaPosicao
                )";
            }
        }

        /// <summary>
        /// Constrói a query de atualização de conferência física baseada no schema
        /// </summary>
        public static string BuildUpdateConferenciaFisicaQuery(ISchemaService schemaService)
        {
            var isRedex = schemaService.GetCurrentSchema() == "REDEX";
            var tableName = schemaService.GetTableName("TB_EFETIVACAO_CONF_FISICA");

            if (isRedex)
            {
                // Para REDEX, NÃO inclui a coluna BL
                return $@"UPDATE {tableName} SET
                    CPF_CONFERENTE = @cpfConferente, 
                    NOME_CONFERENTE = @nomeConferente,
                    TELEFONE_CONFERENTE = @telefoneConferente,
                    CPF_CLIENTE = @cpfCliente, 
                    NOME_CLIENTE = @nomeCliente, 
                    RETIRADA_AMOSTRA = @retiradaAmostra,
                    DIVERGENCIA_QTDE = @quantidadeDivergente,
                    DIVERGENCIA_QUALIFICACAO = @divergenciaQualificacao,
                    OBS_DIVERGENCIA = @observacaoDivergencias,
                    CONFREMOTA = @conferenciaRemota,
                    EMBALAGEM = @embalagem,
                    QTD_REPRESENTANTES = @quantidadeRepresentantes,
                    QTD_AJUDANTES = @quantidadeAjudantes,
                    QTD_OPERADORES = @quantidadeOperadores,
                    MOVIMENTACAO = @movimentacao,
                    DESUNITIZACAO = @desunitizacao,
                    QTD_DOCUMENTOS = @quantidadeDocumentos,
                    QTD_VOLUMES_DIVERGENTES = @quantidadeVolumesDivergentes, 
                    TIPO_CONFERENCIA = @tipo
                 WHERE ID = @ID";
            }
            else
            {
                // Para outros schemas, inclui a coluna BL
                return $@"UPDATE {tableName} SET
                    BL = @bl,
                    CPF_CONFERENTE = @cpfConferente, 
                    NOME_CONFERENTE = @nomeConferente,
                    TELEFONE_CONFERENTE = @telefoneConferente,
                    CPF_CLIENTE = @cpfCliente, 
                    NOME_CLIENTE = @nomeCliente, 
                    RETIRADA_AMOSTRA = @retiradaAmostra,
                    DIVERGENCIA_QTDE = @quantidadeDivergente,
                    DIVERGENCIA_QUALIFICACAO = @divergenciaQualificacao,
                    OBS_DIVERGENCIA = @observacaoDivergencias,
                    CONFREMOTA = @conferenciaRemota,
                    EMBALAGEM = @embalagem,
                    QTD_REPRESENTANTES = @quantidadeRepresentantes,
                    QTD_AJUDANTES = @quantidadeAjudantes,
                    QTD_OPERADORES = @quantidadeOperadores,
                    MOVIMENTACAO = @movimentacao,
                    DESUNITIZACAO = @desunitizacao,
                    QTD_DOCUMENTOS = @quantidadeDocumentos,
                    QTD_VOLUMES_DIVERGENTES = @quantidadeVolumesDivergentes, 
                    TIPO_CONFERENCIA = @tipo
                 WHERE ID = @ID";
            }
        }
    }
}
