using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenciaFisica.Domain.Entities
{
    public class Processo
    {
        public int IdTipoProcesso { get; set; }

        public string ImagemPath { get; set; }

        public string Descricao { get; set; }

        public string Observacao { get; set; }
        public int IdTalie { get; set; }
    }
}
