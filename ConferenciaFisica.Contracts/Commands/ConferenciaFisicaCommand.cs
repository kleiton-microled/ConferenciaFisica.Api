﻿namespace ConferenciaFisica.Contracts.Commands
{
    public class ConferenciaFisicaCommand
    {
        public ConferenciaFisicaCommand(int? id, string? cntr, string? bl, DateTime inicio, DateTime termino, string? cpfConferente, 
            string? nomeConferente, string? cpfCliente, string? nomeCliente, int? qtdeDivergente, string? divergenciaQualificacao, 
            string? observacaoDivergencia, int? retiradaAmostra, bool? conferenciaRemota, string? operacao, 
            int? qtdeVolumesDivergentes, int? qtdeRepresentantes, int? quantidadeAjudantes, int? quantidadeOperadores, 
            string? movimentacao, int? desuniticacao, int? quantidadeDocumentos)
        {
            Id = id;
            Cntr = cntr;
            Bl = bl;
            Inicio = inicio;
            Termino = termino;
            CpfConferente = cpfConferente;
            NomeConferente = nomeConferente;
            CpfCliente = cpfCliente;
            NomeCliente = nomeCliente;
            QuantidadeDivergente = qtdeDivergente;
            DivergenciaQualificacao = divergenciaQualificacao;
            ObservacaoDivergencia = observacaoDivergencia;
            RetiradaAmostra = retiradaAmostra;
            ConferenciaRemota = conferenciaRemota;
            Operacao = operacao;
            QtdeVolumesDivergentes = qtdeVolumesDivergentes;
            QuantidadeRepresentantes = qtdeRepresentantes;
            QuantidadeAjudantes = quantidadeAjudantes;
            QuantidadeOperadores = quantidadeOperadores;
            Movimentacao = movimentacao;
            Desunitizacao = desuniticacao;
            QuantidadeDocumentos = quantidadeDocumentos;
        }
        public int? Id { get; set; }
        public string? Cntr { get; set; }
        public string? Bl { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Termino { get; set; }
        public string? CpfConferente { get; set; }
        public string? NomeConferente { get; set; }
        public string? CpfCliente { get; set; }
        public string? NomeCliente { get; set; }
        public int? QuantidadeDivergente { get; set; }
        public string? DivergenciaQualificacao { get; set; }
        public string? ObservacaoDivergencia { get; set; }
        public int? RetiradaAmostra { get; set; }
        public int Embalagem { get; set; }
        //LACRES
        public bool? ConferenciaRemota { get; set; }
        public string? Operacao { get; set; }
        //modulo?????
        public int? QtdeVolumesDivergentes { get; set; }
        public int? QuantidadeRepresentantes { get; set; }
        public int? QuantidadeAjudantes { get; set; }
        public int? QuantidadeOperadores { get; set; }
        public string? Movimentacao { get; set; }
        public int? Desunitizacao { get; set; }
        public int? QuantidadeDocumentos { get; set; }

        public static ConferenciaFisicaCommand New(int? id, string? cntr, string? bl, DateTime inicio, DateTime termino, string? cpfConferente, string? nomeConferente, 
                                                   string? cpfCliente, string? nomeCliente, int? qtdeDivergente, string? divergenciaQualificacao, 
                                                   string? observacaoDivergencia, int? retiradaAmostra, bool? conferenciaRemota, string    ? operacao, 
                                                   int? qtdeVolumesDivergentes, int? qtdeRepresentantes, int? quantidadeAjudantes, int? quantidadeOperadores, 
                                                   string? movimentacao, int? desuniticacao, int? quantidadeDocumentos)
        {
            return new ConferenciaFisicaCommand(id,cntr, bl, inicio, termino, cpfConferente, nomeConferente, 
                                                cpfCliente, nomeCliente, qtdeDivergente, divergenciaQualificacao, 
                                                observacaoDivergencia, retiradaAmostra, conferenciaRemota, operacao, 
                                                qtdeVolumesDivergentes, qtdeRepresentantes, quantidadeAjudantes, quantidadeOperadores,
                                                movimentacao, desuniticacao,quantidadeDocumentos);
        }
    }
}
