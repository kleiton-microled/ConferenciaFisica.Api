using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface ICarregamentoCargaSoltaRepository
    {
        Task<EnumValueDTO[]?> GetVeiculosByPatio(int patio);
    }
}
