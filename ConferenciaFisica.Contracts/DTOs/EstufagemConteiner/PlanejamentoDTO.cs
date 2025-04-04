namespace ConferenciaFisica.Contracts.DTOs.EstufagemConteiner
{
    public class PlanejamentoDTO
    {
        public string Cliente { get; set; }
        public string Reserva { get; set; }
        public string Conteiner { get; set; }
        public string? Inicio { get; set; }
        public string? Termino { get; set; }
        public int Conferente { get; set; }
        public int Equipe { get; set; }
        public int Operacao { get; set; }
        public int AutonumCamera { get; set; }
        public string DescricaoCamera { get; set; }
        public int AutonumRo { get; set; }
        public int AutonumBoo { get; set; }
        public int AutonumPatio { get; set; }
        public int AutonumTalie { get; set; }
        public int IdTimeline { get; set; }
        public string Yard { get; set; }
        public bool PossuiMarcantes { get; set; }
        public int QtdePlanejada { get; set; }
        public int Plan { get; set; }
        public int Ttl { get; set; }
        public int Produto { get; set; }
    }

}
