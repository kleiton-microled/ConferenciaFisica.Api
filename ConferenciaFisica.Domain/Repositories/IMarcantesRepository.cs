using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IMarcantesRepository
    {
        Task<IEnumerable<Marcante>> BuscarMarcantes(string pesquisa);
        Task<IEnumerable<MovimentacaoCargaDTO>> BuscarCargaParaMovimentar(int idMarcante);
    }
}
