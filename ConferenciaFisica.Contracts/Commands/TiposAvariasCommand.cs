namespace ConferenciaFisica.Contracts.Commands
{
    public class TiposAvariasCommand
    {
        public TiposAvariasCommand(int id, string codigo, string descricao)
        {
            Id = id;
            Codigo = codigo;
            Descricao = descricao;
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public static TiposAvariasCommand New(int id, string codigo, string descricao)
        {
            return new TiposAvariasCommand(id, codigo, descricao);
        }
    }
}
