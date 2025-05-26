using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.Inputs;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.Avarias.Interface
{
    public interface IAvariasConferenciaUseCase
    {
        Task<ServiceResult<AvariasConferencia>> BuscarAvariasConferencia(int idConferencia);
        Task<ServiceResult<bool>> CadastrarAvariaConferencia(AvariaConferenciaInput input);

    }
}
