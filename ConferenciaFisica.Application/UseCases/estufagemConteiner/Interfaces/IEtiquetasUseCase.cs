using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;

namespace ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces
{
    public interface IEtiquetasUseCase
    {
        Task<ServiceResult<IEnumerable<EtiquetaDTO>>> BuscarEtiquetas(int planejamento);
    }
}
