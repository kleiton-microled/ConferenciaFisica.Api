using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IConferenciaRepository
    {
        Task<Conferencia> BuscarPorConteinerAsync(string idConteiner);
        Task<Conferencia> BuscarPorLoteAsync(string idLote);
        Task<Conferencia> BuscarPorReservaAsync(string idLote);
        Task<bool> IniciarConferencia(ConferenciaFisicaCommand command);
        Task<bool> AtualizarConferencia(ConferenciaFisicaCommand command);
        Task<bool> CadastroAdicional(CadastroAdicionalCommand command);
        Task<bool> Delete(int id);
        Task<IEnumerable<CadastrosAdicionaisDTO>> CarregarCadastrosAdicionais(int idReferencia);
        Task<IEnumerable<TipoLacre>> CarregarTiposLacres();
        Task<IEnumerable<Lacre>> CarregarLacresConferencia(int idConferencia);
        Task<bool> CadastroLacreConferencia(LacreConferenciaCommand command);
        Task<bool> AtualizarLacreConferencia(LacreConferenciaCommand command);
        Task<bool> ExcluirLacreConferencia(int id);

        //
        Task<IEnumerable<DocumentosConferencia>> CarregarDocumentosConferencia(int idConferencia);
        Task<bool> CadastroDocumentosConferencia(DocumentoConferenciaCommand command);
        Task<bool> AtualizarDocumentosConferencia(DocumentoConferenciaCommand command);
        Task<bool> ExcluirDocumentosConferencia(int id);

    }
}
