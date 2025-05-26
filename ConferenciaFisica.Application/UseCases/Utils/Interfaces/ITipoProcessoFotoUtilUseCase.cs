using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.DTOs;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Utils.Interfaces
{
    public interface ITipoProcessoFotoUtilUseCase : IUtilUseCase<TipoProcessoFotoModel, TipoProcessoFotoViewModel>
    {
        Task<ServiceResult<IEnumerable<EnumValueDTO>>> GetByProcessoNome(string processoName);
    }
}
