﻿namespace ConferenciaFisica.Contracts.DTOs
{
    public class TalieItemDTO
    {
        public int Id { get; set; }
        public string NotaFiscal { get; set; }
        public string Embalagem { get; set; }
        public int QuantidadeNf { get; set; }
        public int QuantidadeDescarga { get; set; }
    }
}
