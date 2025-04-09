using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Application.UseCases.CarregamentoCargaSolta.Interface
{
    public interface ICarregamentoCargaSoltaUseCase
    {
        public Task<ServiceResult<EnumValueDTO[]>> GetVeiculos(int patio);

        public Task<ServiceResult<EnumValueDTO>> GetOrdens(int patio, string veiculo, string local, int quantidade, DateTime inicio);


    }
}
