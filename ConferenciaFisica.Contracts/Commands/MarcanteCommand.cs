﻿namespace ConferenciaFisica.Contracts.Commands
{
    public class MarcanteCommand
    {
        public int Registro { get; set; }
        public int TalieId { get; set; }
        public int TalieItemId { get; set; }
        public string Marcante { get; set; }
        public int Quantidade { get; set; }
        public int Armazem { get; set; }
        public string Local { get; set; }

        //public static MarcanteCommand Create(int talieId, int talieItemId, int marcante, int quantidade, int armazen, string local)
        //{

        //}
    }
}
