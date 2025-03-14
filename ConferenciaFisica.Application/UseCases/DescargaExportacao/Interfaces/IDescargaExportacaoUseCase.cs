using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Application.ViewModels;

namespace ConferenciaFisica.Application.UseCases.DescargaExportacao.Interfaces
{
    public interface IDescargaExportacaoUseCase
    {
        Task<ServiceResult<DescargaExportacaoViewModel>> BuscarPorRegistro(int registro);
        Task<ServiceResult<bool>> GravarOuAtualizarTalie(DescargaExportacaoViewModel request);
        Task<ServiceResult<bool>> CadastrarAvaria(AvariaInput input);
        Task<ServiceResult<TalieItemViewModel>> BuscarItemTalie(int id);
        Task<ServiceResult<bool>> SalvarTalieItem(TalieItemViewModel request, int registro);
        Task<ServiceResult<bool>> ExcluirTalieItem(int id);

    }
}
