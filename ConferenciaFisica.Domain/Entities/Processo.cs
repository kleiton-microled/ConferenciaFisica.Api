namespace ConferenciaFisica.Domain.Entities
{
    public class Processo
    {
        public int IdTipoProcesso { get; set; }

        public string ImagemPath { get; set; }

        public string Descricao { get; set; }

        public string Observacao { get; set; }
        public string DescricaoTipoProcesso { get; set; }
        public int IdTalie { get; set; }
        public string? IdContainer { get; set; }
        public int Id { get; set; }


    }
}
