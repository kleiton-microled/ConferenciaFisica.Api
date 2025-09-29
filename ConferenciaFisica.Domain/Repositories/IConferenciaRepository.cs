using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IConferenciaRepository
    {
        Task<Conferencia> BuscarPorConteinerAsync(string idConteiner);
        Task<Conferencia> BuscarPorConteinerRedexAsync(string idConteiner);
        Task<Conferencia> BuscarPorLoteAsync(string idLote);
        Task<Conferencia> BuscarPorPorId(int id);
        Task<Conferencia> BuscarPorReservaAsync(string idLote);
        Task<bool> IniciarConferencia(ConferenciaFisicaCommand command);
        Task<bool> IniciarConferencia(
            string? cntr, string? bl, string? cpfConferente, string? nomeConferente, 
            string? telefoneConferente, string? cpfCliente, string? nomeCliente, 
            int? quantidadeDivergente, bool divergenciaQualificacao, string? observacaoDivergencias, 
            int? retiradaAmostra, bool? conferenciaRemota, int? quantidadeVolumesDivergentes, 
            int? quantidadeRepresentantes, int? quantidadeAjudantes, int? quantidadeOperadores, 
            int? movimentacao, int? desunitizacao, int? quantidadeDocumentos, int? autonumAgendaPosicao);
        Task<bool> AtualizarConferencia(ConferenciaFisicaCommand command);
        Task<bool> CadastroAdicional(CadastroAdicionalCommand command);
        Task<bool> Delete(int id, int? idConferencia = 0, string? Tipo = "");
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
        Task<bool> ExcluirDocumentosConferencia(int id, int? idConferencia);
        Task<bool> FinalizarConferencia(int idConferencia);
        Task<string> BuscarCpfConferente(string conferente);

    }
}
