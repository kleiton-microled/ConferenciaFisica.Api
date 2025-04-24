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
        Task<IEnumerable<Marcante>> CarregarMarcantesTalieItem(int talieItemId);
        Task<bool> GravarMarcante(MarcanteCommand input);
        Task<bool> ExcluirMarcanteTalieItem(int id);
        Task<bool> FinalizarProcesso(int id);
        Task<bool> FinalizarProcessoCrossDocking(int talieId, string conteiner);
        Task<bool> ValidarQuantidadeDescargaAsync(int talieId);
        Task<bool> VerificarEmissaoEtiquetasAsync(int talieId);
        Task<bool> ValidarCargaTransferidaAsync(int talieId);
        Task<bool> FecharTalieAsync(int talieId);
        Task FinalizarReservaAsync(int booId);
        Task<IEnumerable<Yard>> BuscarYard(string pesquisa);
        Task<IEnumerable<PatioCsCrossDock>> BuscarTalieCrossDock(int id);
        Task<bool> CrossDockUpdatePatioF(string conteiner);
        Task<int?> CrossDockGetNumeroReservaContainer(string conteiner);
        Task<int?> GetCrossDockRomaneioId(int patioContainer);
        Task<int> GetCrossDockSequencialId();
        //Task<int> InserirRomaneio(int romaneioId, string usuario, int container, int reservaContainer);
        //Task InserirRomaneioCargaSolta(int romaneioId, int autonumPcs, decimal qtdeEntrada);
        Task<int?> CrossDockBuscarTaliePorContainer(int patioContainer);
        Task<DateTime> CrossDockGetDataInicoEstufagem(int patioContainer);
        Task<DateTime> CrossDockGetDataFimEstufagem(int patioContainer);
        Task CrossDockUpdateTalieItem(DateTime dataInicioEstufagem, DateTime dataFimEstufagem, int patioContainer);
        //Task UpdateRomaneio(int talieCarregamento, int idRomaneio);
        Task<int?> CrossDockCriarTalie(int patioContainer, DateTime dataInicioEstufagem, DateTime dataFimEstufagem, int reservaContainer, int romaneioId, string operacao);
        Task InserirSaidaNF(int patioContainer, int numeroNf, int quantidadeEstufada);
        Task CrossDockAtualizarQuantidadeEstufadaNF(int numeroNf, int quantidadeEstufada);
        Task<int> GetQuantidadeSaidaCarga(int autonumPcs);
        Task UpdatepatioCsFlag(int autonumPcs);
        Task<int?> CrossDockGetLastTalie();
        Task CrossDockInserirSaidaCarga(int autonumPcs, decimal qtdeEntrada, int autonumEmb, decimal bruto, decimal altura, decimal comprimento, decimal largura, decimal volumeDeclarado, int patioContainer, string v, int autonumNf, int? talieByContainer, int romaneioId);
    }
}
