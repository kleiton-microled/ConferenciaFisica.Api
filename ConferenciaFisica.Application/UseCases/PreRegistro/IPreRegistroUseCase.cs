using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Entities.PreRegistro;

namespace ConferenciaFisica.Application.UseCases.PreRegistro
{
    public interface IPreRegistroUseCase
    {
        Task<ServiceResult<bool>> Cadastrar(SaidaCaminhaoViewModel input);
        Task<ServiceResult<DadosAgendamentoModel>> GetDadosAgendamento(PreRegistroInput input);
    }
}
