using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Domain.Repositories.EstufagemConteiner;

namespace ConferenciaFisica.Application.UseCases.estufagemConteiner
{
    
    public class PlanejamentoUseCase : IPlanejamentoUseCase
    {
        private readonly IEstufagemConteinerRepository _estufagemConteinerRepository;

        public PlanejamentoUseCase(IEstufagemConteinerRepository estufagemConteinerRepository)
        {
            _estufagemConteinerRepository = estufagemConteinerRepository;
        }

        public async Task<ServiceResult<PlanejamentoDTO>> BuscarPlanejamento(int planejamento)
        {
            var data = await _estufagemConteinerRepository.BuscarPlanejamento(planejamento);
            if (data == null)
            {
                return ServiceResult<PlanejamentoDTO>.Success(data, "planejamento não encontrada.");
            }

            return ServiceResult<PlanejamentoDTO>.Success(data, "Planejamento localizado com sucesso.");
        }
    }
}
