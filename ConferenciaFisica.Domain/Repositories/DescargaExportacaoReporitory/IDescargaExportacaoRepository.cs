using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Entities.DescargaExportacao;

namespace ConferenciaFisica.Domain.Repositories.DescargaExportacaoReporitory
{
    public interface IDescargaExportacaoRepository
    {
        Task<DescargaExportacao> BuscarRegistroAsync(int registro);
        Task<int> AtualizarOuCriarTalie(DescargaExportacaoCommand command);
        Task<IEnumerable<TalieItem>> BuscarTaliItens(int id);
        Task GerarDescargaAutomatica(int registro, int talieId);
        Task<int> CadastrarAvaria(CadastroAvariaCommand command);
        Task<int> AtualizarAvaria(CadastroAvariaCommand command);
        Task<TalieItem> BuscarTalieItem(int id);
        Task<bool> UpdateTalieItem(TalieItem item);
        Task<bool> CadastrarTalieItem(TalieItem item, int registro);
        Task<bool> ExcluirTalieItem(int id);
        Task<bool> GravarObservacao(string observacao, int talieId);
        Task<IEnumerable<Armazens>> CarregarArmazens(int patio);
        Task<bool> GravarMarcante(MarcanteCommand input);

    }
}
