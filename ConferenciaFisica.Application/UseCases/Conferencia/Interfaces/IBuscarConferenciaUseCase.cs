using ConferenciaFisica.Application.Common.Models;

namespace ConferenciaFisica.Application.UseCases.Conferencia.Interfaces
{
    public interface IBuscarConferenciaUseCase
    {
        Task<ServiceResult<Domain.Entities.Conferencia>> ExecuteAsync(string idConteiner, string idLote);
    }
}
