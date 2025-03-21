using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface IImagemRepository
    {
        Task<IEnumerable<TipoFotoModel>> GetImagesTypes();
        Task<bool> CreateTipoFoto(TipoFotoModel input);
        Task<bool> UpdateTipoFoto(TipoFotoModel input);
        Task<bool> DeleteTipoFoto(int id);

        Task<bool> CreateProcesso(ProcessoCommand input);
        Task<bool> UpdateProcesso(ProcessoCommand input);
        Task<bool> DeleteProcesso(int id);
        
        Task<IEnumerable<Processo>> ListProcessoByTalieId(int id);
    }
}
