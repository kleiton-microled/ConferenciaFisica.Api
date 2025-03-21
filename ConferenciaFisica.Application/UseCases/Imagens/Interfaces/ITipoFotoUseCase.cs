using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Imagens.Interfaces
{
    
    public interface ITipoFotoUseCase
    {
        Task<ServiceResult<IEnumerable<TipoFotoModel>>> GetAllTipoFoto();
        Task<ServiceResult<bool>> CreateTipoFoto(TipoFotoViewModel input);
        Task<ServiceResult<bool>> UpdateTiposFoto(UpdateTiposFotoViewModel input);
        Task<ServiceResult<bool>> UpdateTipoFoto(UpdateTipoFotoViewModel input);
        Task<ServiceResult<bool>> DeleteTipoFoto(int id);

        Task<ServiceResult<bool>> InsertProcesso(ProcessoViewModel input);
        
        Task<ServiceResult<bool>> DeleteProcesso(int id);

        Task<ServiceResult<IEnumerable<ProcessoViewModel>>> GetImagemByTalieId(int registroId);
    }
} 