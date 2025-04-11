using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenciaFisica.Domain.Entities
{
    public class MarcantePatioModel
    {
        public int MARCANTE { get; set; }
        public int AUTONUMCS { get; set; }
        public string LOTE { get; set; }
        public string ITEM { get; set; }
        public decimal QUANTIDADE { get; set; }
        public string EMBALAGEM { get; set; }
        public string MERCADORIA { get; set; }
        public string MARCA { get; set; }
        public int AUTONUM_ARMAZEM { get; set; }
        public string DESCR_ARMAZEM { get; set; }
        public int AUTONUMCNTR { get; set; }
        public string Id_Conteiner { get; set; }
        public string POSICAO { get; set; }
        public int AUTONUM_CS_YARD { get; set; }

    }
}
