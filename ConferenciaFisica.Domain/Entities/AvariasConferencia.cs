using System.Collections.Generic;

namespace ConferenciaFisica.Domain.Entities
{
    public class AvariasConferencia
    {
        public int Id { get; set; }
        public int IdConferencia { get; set; }
        public int QuantidadeAvariada { get; set; }
        public decimal PesoAvariado { get; set; }
        public int IdEmbalagem { get; set; }
        public string Conteiner { get; set; }
        public string Observacao { get; set; }
        public List<TiposAvarias> TiposAvarias { get; set; }
    }
}
