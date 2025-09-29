namespace ConferenciaFisica.Contracts.Commands
{
    public class ConferenciaFisicaCommand
    {
        public ConferenciaFisicaCommand(int? id, int? tipo, int? cntr, string? bl, DateTime inicio, DateTime termino, string? cpfConferente, 
            string? nomeConferente, string? telefoneConferente, string? cpfCliente, string? nomeCliente, int? qtdeDivergente, bool divergenciaQualificacao, 
            string? observacaoDivergencias, int? retiradaAmostra, int? embalagem, bool? conferenciaRemota, string? operacao, 
            int? qtdeVolumesDivergentes, int? qtdeRepresentantes, int? quantidadeAjudantes, int? quantidadeOperadores, 
            int? movimentacao, int? desuniticacao, int? porcentagemDesunitizacao, int? quantidadeDocumentos, int? autonumAgendaPosicao)
        {
            Id = id;
            Tipo = tipo;
            Cntr = cntr;
            Bl = bl;
            Inicio = inicio;
            Termino = termino;
            CpfConferente = cpfConferente;
            NomeConferente = nomeConferente;
            TelefoneConferente = telefoneConferente;
            CpfCliente = cpfCliente;
            NomeCliente = nomeCliente;
            QuantidadeDivergente = qtdeDivergente;
            DivergenciaQualificacao = divergenciaQualificacao;
            ObservacaoDivergencias = observacaoDivergencias;
            RetiradaAmostra = retiradaAmostra;
            Embalagem = embalagem;
            ConferenciaRemota = conferenciaRemota;
            Operacao = operacao;
            QuantidadeVolumesDivergentes = qtdeVolumesDivergentes;
            QuantidadeRepresentantes = qtdeRepresentantes;
            QuantidadeAjudantes = quantidadeAjudantes;
            QuantidadeOperadores = quantidadeOperadores;
            Movimentacao = movimentacao;
            Desunitizacao = desuniticacao;
            PorcentagemDesunitizacao = porcentagemDesunitizacao;
            QuantidadeDocumentos = quantidadeDocumentos;
            AutonumAgendaPosicao = autonumAgendaPosicao;
        }
        public int? Id { get; set; }
        public int? Tipo { get; set; }
        public int? Cntr { get; set; }
        public string? Bl { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Termino { get; set; }
        public string? CpfConferente { get; set; }
        public string? NomeConferente { get; set; }
        public string? TelefoneConferente { get; set; }
        public string? CpfCliente { get; set; }
        public string? NomeCliente { get; set; }
        public int? QuantidadeDivergente { get; set; }
        public bool DivergenciaQualificacao { get; set; }
        public string? ObservacaoDivergencias { get; set; }
        public int? RetiradaAmostra { get; set; }
        public int? Embalagem { get; set; }
        //LACRES
        public bool? ConferenciaRemota { get; set; }
        public string? Operacao { get; set; }
        //modulo?????
        public int? QuantidadeVolumesDivergentes { get; set; } = 0;
        public int? QuantidadeRepresentantes { get; set; } = 0;
        public int? QuantidadeAjudantes { get; set; } = 0;
        public int? QuantidadeOperadores { get; set; } = 0;
        public int? Movimentacao { get; set; }
        public int? Desunitizacao { get; set; }
        public int? PorcentagemDesunitizacao { get; set; }
        public int? QuantidadeDocumentos { get; set; }
        public int? AutonumAgendaPosicao { get; set; }

        public static ConferenciaFisicaCommand New(int? id, int? tipo, int? cntr, string? bl, DateTime inicio, DateTime termino, string? cpfConferente, string? nomeConferente, 
                                                   string? telefoneConferente, string? cpfCliente, string? nomeCliente, int? qtdeDivergente, bool divergenciaQualificacao, 
                                                   string? observacaoDivergencias, int? retiradaAmostra, int? embalagem, bool? conferenciaRemota, string    ? operacao, 
                                                   int? qtdeVolumesDivergentes, int? qtdeRepresentantes, int? quantidadeAjudantes, int? quantidadeOperadores, 
                                                   int? movimentacao, int? desuniticacao, int? porcentagemDesunitizacao, int? quantidadeDocumentos, int? autonumAgendaPosicao)
        {
            return new ConferenciaFisicaCommand(id,tipo,cntr, bl, inicio, termino, cpfConferente, nomeConferente, telefoneConferente, 
                                                cpfCliente, nomeCliente, qtdeDivergente, divergenciaQualificacao, 
                                                observacaoDivergencias, retiradaAmostra, embalagem, conferenciaRemota, operacao, 
                                                qtdeVolumesDivergentes, qtdeRepresentantes, quantidadeAjudantes, quantidadeOperadores,
                                                movimentacao, desuniticacao, porcentagemDesunitizacao, quantidadeDocumentos, autonumAgendaPosicao);
        }
    }
}
