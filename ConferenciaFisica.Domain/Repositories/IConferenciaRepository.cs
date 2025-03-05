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
    }
}
