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
        Task<ServiceResult<bool>> GravarObservacao(string observacao, int talieId);
        Task<ServiceResult<IEnumerable<ArmazensViewModel>>> CarregarArmazens(int patio);
        Task<ServiceResult<IEnumerable<MarcantesViewModel>>> CarregarMarcantes(int talieItem);
        Task<ServiceResult<bool>> GravarMarcante(MarcanteInput input);
        Task<ServiceResult<bool>> ExcluirTalieItem(int registroId, int talieId);
        Task<ServiceResult<bool>> ExcluirMarcanteTalieItem(int id);
        Task<ServiceResult<bool>> FinalizarProcesso(int id);


    }
}
