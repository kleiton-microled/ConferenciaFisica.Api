using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IImagemRepository
    {
        Task<IEnumerable<TipoProcesso>> GetImagesTypes();
        Task<bool> CreateTipoProcesso(TipoProcesso input);
        Task<bool> CreateProcesso(ProcessoCommand input);
        Task<bool> DeleteTipoProcesso(int id);
    }
}
