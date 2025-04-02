using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Entities;

namespace ConferenciaFisica.Application.UseCases.SaidaCaminhao.Interfaces
{
    public interface ISaidaDeCaminhaoUseCase
    {
        public Task<ServiceResult<RegistroSaidaCaminhaoDTO>> GetDadosCaminhao(string protocolo, string ano, string placa, string placaCarreta, int patio);

        public Task<ServiceResult<bool>> RegistrarSaida(SaidaCaminhaoViewModel input);
    }
}
