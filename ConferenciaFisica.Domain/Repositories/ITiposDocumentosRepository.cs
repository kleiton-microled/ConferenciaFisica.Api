using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface ITiposDocumentosRepository
    {
        Task<IEnumerable<TipoDocumentos>> GetAll();
    }
}
