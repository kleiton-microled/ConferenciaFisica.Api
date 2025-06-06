﻿using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.estufagemConteiner.Interfaces;
using ConferenciaFisica.Contracts.DTOs.EstufagemConteiner;
using ConferenciaFisica.Domain.Entities.DescargaExportacao;
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

        public async Task<ServiceResult<bool>> IniciarEstufagem(TalieInsertDTO talie)
        {
            var _serviceResult = new ServiceResult<bool>();

            var result = await _repository.IniciarEstufagem(talie);
            if (result)
            {
                _serviceResult.Result = result;
                _serviceResult.Mensagens.Add("Serviço Iniciado com sucesso!");
            }
            else
            {
                _serviceResult.Mensagens.Add("Falha ao tentar iniciar!");
            }

            return _serviceResult;
        }

        public async Task<ServiceResult<bool>> Estufar(SaldoCargaMarcanteDto request)
        {
            var _serviceResult = new ServiceResult<bool>();

            var result = await _repository.Estufar(request);
            if (result)
            {
                _serviceResult.Result = result;
                _serviceResult.Mensagens.Add("Carga Estufada com sucesso!");
            }
            else
            {
                _serviceResult.Mensagens.Add("Falha ao tentar estufar!");
            }

            return _serviceResult;
        }

        public async Task<ServiceResult<bool>> Finalizar(TalieInsertDTO talie)
        {
            var _serviceResult = new ServiceResult<bool>();

            var result = await _repository.Finalizar(talie);
            if (result)
            {
                _serviceResult.Result = result;
                _serviceResult.Mensagens.Add("Serviço Finalizado com sucesso!");
            }
            else
            {
                _serviceResult.Mensagens.Add("Falha ao tentar finalizar!");
            }

            return _serviceResult;
        }
    }
}
