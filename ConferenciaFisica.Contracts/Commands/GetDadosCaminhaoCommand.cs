namespace ConferenciaFisica.Contracts.Commands
{
    public class GetDadosCaminhaoCommand
    {
        public string Protocolo { get; set; }
        public string Ano { get; set; }
        public string Placa { get; set; }
        public string PlacaCarreta { get; set; }
        public int LocalPatio { get; set; }
    }
}
