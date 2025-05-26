using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Documentos.Interfaces
{
    public interface ITiposDocumentosUseCase
    {
        Task<ServiceResult<IEnumerable<TipoDocumentos>>> GetAllAsync();
    }
}
