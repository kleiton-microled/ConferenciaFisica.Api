using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;
using ConferenciaFisica.Domain.Repositories;
using ConferenciaFisica.Domain.Repositories.EstufagemConteiner;

namespace ConferenciaFisica.Application.UseCases.estufagemConteiner
{
    
    public class PlanejamentoUseCase : IPlanejamentoUseCase
    {
        private readonly IEstufagemConteinerRepository _repository;

        public PlanejamentoUseCase(IEstufagemConteinerRepository estufagemConteinerRepository)
        {
            _repository = estufagemConteinerRepository;
        }

        public async Task<ServiceResult<PlanejamentoDTO>> BuscarPlanejamento(int planejamento)
        {
            var data = await _repository.BuscarPlanejamento(planejamento);
            if (data == null)
            {
                return ServiceResult<PlanejamentoDTO>.Success(data, "planejamento não encontrada.");
            }

            return ServiceResult<PlanejamentoDTO>.Success(data, "Planejamento localizado com sucesso.");
        }

        public async Task<ServiceResult<SaldoCargaMarcanteDto>> BuscarSaldoCargaMarcante(int planejamento, string codigoMarcante)
        {
            var data = await _repository.BuscarSaldoCargaMarcante(planejamento, codigoMarcante);
            if (data == null)
            {
                return ServiceResult<SaldoCargaMarcanteDto>.Success(data, "Dados não encontrados.");
            }

            return ServiceResult<SaldoCargaMarcanteDto>.Success(data, "Dados localizado com sucesso.");
        }
    }
}
