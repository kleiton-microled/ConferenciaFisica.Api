using ConferenciaFisica.Contracts.DTOs;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IColetorRepository
    {
        Task<IEnumerable<ConferenteDTO>> ListarConferentes();
    }
}
