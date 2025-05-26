using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.ViewModels
{
    public class CarregamentoOrdem
    {
        public CarregamentoOrdemModel[] Ordens { get; set; }
        public ItemCarregadoModel[] ItensCarregados { get; set; }
    }
}
