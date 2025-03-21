using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface ITiposProcessoFotoRepository : IBaseRepository<TipoProcessoFotoModel>
    {
        Task<IEnumerable<TipoProcessoFotoModel>> GetByProcessoNome(string processoName);
    }
}
