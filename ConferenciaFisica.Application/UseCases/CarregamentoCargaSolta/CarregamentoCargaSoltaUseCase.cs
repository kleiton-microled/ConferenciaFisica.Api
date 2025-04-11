using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.CarregamentoCargaSolta.Interface;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.CarregamentoCargaSolta
{
    public class CarregamentoCargaSoltaUseCase : ICarregamentoCargaSoltaUseCase
    {
        private readonly ICarregamentoCargaSoltaRepository _carregamentoCargaSoltaRepository;

        public CarregamentoCargaSoltaUseCase(ICarregamentoCargaSoltaRepository carregamentoCargaSoltaRepository)
        {
            _carregamentoCargaSoltaRepository = carregamentoCargaSoltaRepository;
        }

        public Task<ServiceResult<EnumValueDTO>> GetOrdens(int patio, string veiculo, string local, int quantidade, DateTime inicio)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<EnumValueDTO[]>> GetVeiculos(int patio)
        {
            try
            {
                var result = await _carregamentoCargaSoltaRepository.GetVeiculosByPatio(patio);

                return result is null ? ServiceResult<EnumValueDTO[]>.Failure("Veiculos nao encontrados") : ServiceResult<EnumValueDTO[]>.Success(result);
            }
            catch (Exception exception)
            {
                return ServiceResult<EnumValueDTO[]>.Failure(exception.Message);
            }
        }
    }
}
