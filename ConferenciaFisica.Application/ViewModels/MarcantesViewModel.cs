﻿namespace ConferenciaFisica.Application.ViewModels
{
    public class MarcantesViewModel
    {
        public int Id { get; set; }
        public string DataImpressao { get; set; }
        public string DataAssociacao { get; set; }
        public int Quantidade { get; set; }
        public int TalieId { get; set; }
        public int TalieItemId { get; set; }
        public string Numero { get; set; }
        public string Local { get; set; }
        public int Armazem
        {
            get; set;
        }
    }
}
