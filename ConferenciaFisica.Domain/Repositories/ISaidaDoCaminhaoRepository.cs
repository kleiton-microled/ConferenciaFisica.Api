using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Enums;

namespace ConferenciaFisica.Domain.Repositories
{
    public interface ISaidaDoCaminhaoRepository
    {
        public Task<RegistroSaidaCaminhaoDTO> GetDadosCaminhao(GetDadosCaminhaoCommand getDadosCaminhaoCommand);
        public Task<bool> RegistrarSaida(RegistrarSaidaCaminhaoCommand registrarSaidaCaminhaoCommand, LocalPatio localPatio);
    }
}
