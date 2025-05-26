using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;

namespace ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces
{
    public interface IItensEstufadosUseCase
    {
        Task<ServiceResult<IEnumerable<ItensEstufadosDTO>>> BuscarItensEstufados(int patio);
    }
}
