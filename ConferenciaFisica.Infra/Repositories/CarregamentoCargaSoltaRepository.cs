using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Infra.Repositories
{
    public class CarregamentoCargaSoltaRepository : ICarregamentoCargaSoltaRepository
    {
        public Task<EnumValueDTO[]?> GetVeiculosByPatio(int patio)
        {
            throw new NotImplementedException();
        }
    }
}
