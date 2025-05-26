using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IEmbalagensRepository
    {
        Task<IEnumerable<TiposEmbalagens>> GetAll();
    }
}
