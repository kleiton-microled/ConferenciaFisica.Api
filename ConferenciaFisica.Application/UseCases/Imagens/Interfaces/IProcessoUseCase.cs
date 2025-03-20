using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Imagens.Interfaces
{
    
    public interface IProcessoUseCase
    {
        Task<ServiceResult<IEnumerable<TipoProcesso>>> ListTipoProcesso();
        Task<ServiceResult<bool>> CreateTipoProcesso(TipoProcessoViewModel input);
        Task<ServiceResult<bool>> DeleteTipoProcesso(int id);

        Task<ServiceResult<bool>> InsertProcesso(ProcessoViewModel input);
        Task<ServiceResult<bool>> UpdateProcesso(UpdateProcessoViewModel input);
        Task<ServiceResult<bool>> DeleteProcesso(int id);

        Task<ServiceResult<IEnumerable<ProcessoViewModel>>> GetImagemByTalieId(int registroId);
    }
} 