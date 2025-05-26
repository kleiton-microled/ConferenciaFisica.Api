namespace ConferenciaFisica.Domain.Entities
{
    public class TipoProcessoFotoModel
    {
        public  int Id { get; set; }
        public  int TipoProcessoID { get; set; }
        public int TipoFotoID { get; set; }
        public  bool Ativo { get; set; }
        public  string Descricao { get; set; }
    }
}
