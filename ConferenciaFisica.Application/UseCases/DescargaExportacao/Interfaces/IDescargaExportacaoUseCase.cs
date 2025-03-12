using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.ViewModels;

namespace ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces
{
    public interface IDescargaExportacaoUseCase
    {
        Task<ServiceResult<DescargaExportacaoViewModel>> BuscarPorRegistro(int registro);
    }
}
