using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IAvariasRepository
    {
        Task<IEnumerable<TiposAvarias>> CarregarTiposAvarias();
        Task<AvariasConferencia> BuscarAvariasConferencia(int idConferencia);
        Task<int> CadastrarAvariaConferencia(CadastroAvariaConferenciaCommand command);
        Task<int> AtualizarAvariaConferencia(CadastroAvariaConferenciaCommand command);
    }
}
