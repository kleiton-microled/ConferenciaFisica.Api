using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Documentos.Interfaces
{
    public interface IDocumentoConferenciaUseCase
    {
        //listar
        Task<ServiceResult<IEnumerable<DocumentosConferencia>>> GetAllAsync(int idConferencia);
        //delete
        Task<ServiceResult<bool>> DeleteAsync(int id, int? idConferencia = 0);
        //insert
        Task<ServiceResult<bool>> ExecuteAsync(DocumentoConferenciaInput input);
        //update
        Task<ServiceResult<bool>> UpdateAsync(DocumentoConferenciaInput input);

    }
}
