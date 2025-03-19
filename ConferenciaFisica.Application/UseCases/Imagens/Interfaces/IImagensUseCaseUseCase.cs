using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Imagens.Interfaces
{
    
    public interface IImagensUseCaseUseCase
    {
        Task<ServiceResult<IEnumerable<TipoProcesso>>> ListTipoProcesso();
        Task<ServiceResult<bool>> CreateTipoProcesso(TipoProcessoViewModel input);
        Task<ServiceResult<bool>> DeleteTipoProcesso(int id);

        Task<ServiceResult<bool>> InsertProcesso(ProcessoViewModel input);

        Task<ServiceResult<IEnumerable<TipoProcesso>>> GetImagemByRegistroId(int registroId);
        Task<ServiceResult<IEnumerable<TipoProcesso>>> UpdateImagemByRegistroId(int registroId);
    }
} 