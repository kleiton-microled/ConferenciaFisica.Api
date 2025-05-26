using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;
using ConferenciaFisica.Domain.Repositories.EstufagemConteiner;

namespace ConferenciaFisica.Application.UseCases.estufagemConteiner
{

    public class ItensEstufadosUseCase : IItensEstufadosUseCase
    {
        private readonly IEstufagemConteinerRepository _estufagemConteinerRepository;

        public ItensEstufadosUseCase(IEstufagemConteinerRepository estufagemConteinerRepository)
        {
            _estufagemConteinerRepository = estufagemConteinerRepository;
        }

        public async Task<ServiceResult<IEnumerable<ItensEstufadosDTO>>> BuscarItensEstufados(int patio)
        {
            var data = await _estufagemConteinerRepository.BuscarItensEstufados(patio);
            if (data == null)
            {
                return ServiceResult<IEnumerable<ItensEstufadosDTO>>.Success(data, "Nenhum registro encontrado.");
            }

            return ServiceResult<IEnumerable<ItensEstufadosDTO>>.Success(data, "Registros localizados com sucesso.");
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
