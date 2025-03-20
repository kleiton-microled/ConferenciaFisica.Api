using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IImagemRepository
    {
        Task<IEnumerable<TipoProcesso>> GetImagesTypes();
        Task<bool> CreateTipoProcesso(TipoProcesso input);
        Task<bool> CreateProcesso(ProcessoCommand input);
        Task<bool> UpdateProcesso(ProcessoCommand input);
        Task<bool> DeleteProcesso(int id);
        Task<bool> DeleteTipoProcesso(int id);
        Task<IEnumerable<Processo>> ListProcessoByTalieId(int id);
    }
}
