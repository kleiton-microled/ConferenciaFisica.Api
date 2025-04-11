using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Application.UseCases.CarregamentoCargaSolta.Interface
{
    public interface ICarregamentoCargaSoltaUseCase
    {
        public Task<ServiceResult<EnumValueDTO[]>> GetVeiculos(int patio);

        public Task<ServiceResult<CarregamentoOrdem>> GetOrdens(int? patio, string? veiculo, string? local, int? quantidade, DateTime? inicio, string tipo = "I");
        Task<ServiceResult<object>> BuscarMacantes(int marcante, int? patio);
        Task<ServiceResult<object>> SalvarMacantes(int marcante, int? patio, string local, string placa);
    }
}
