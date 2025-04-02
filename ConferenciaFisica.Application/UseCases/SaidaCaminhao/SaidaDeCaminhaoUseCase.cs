using System.IO;
using System.Numerics;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.UseCases.SaidaCaminhao.Interfaces;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Contracts.Commands;
using ConferenciaFisica.Domain.Entities;
using ConferenciaFisica.Domain.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConferenciaFisica.Application.UseCases.SaidaCaminhao
{
    public class SaidaDeCaminhaoUseCase : ISaidaDeCaminhaoUseCase
    {
        private readonly ISaidaDoCaminhaoRepository _saidaDeCaminhaoRepository;

        public SaidaDeCaminhaoUseCase(ISaidaDoCaminhaoRepository saidaDeCaminhaoRepository)
        {
            _saidaDeCaminhaoRepository = saidaDeCaminhaoRepository;
        }

        public async Task<ServiceResult<RegistroSaidaCaminhaoDTO>> GetDadosCaminhao(string protocolo, string ano, string placa, string placaCarreta, int patio)
        {
            try
            {
                var result = await _saidaDeCaminhaoRepository.GetDadosCaminhao(new GetDadosCaminhaoCommand
                {
                    Protocolo = protocolo,
                    Ano = ano,
                    Placa = placa,
                    PlacaCarreta = placaCarreta,
                    LocalPatio = patio
                });

                return ServiceResult<RegistroSaidaCaminhaoDTO>.Success(result);
            }
            catch (Exception exception)
            {
                return ServiceResult<RegistroSaidaCaminhaoDTO>.Failure(exception.Message);
            }
        }

        public async Task<ServiceResult<bool>> RegistrarSaida(SaidaCaminhaoViewModel input)
        {
            try
            {
                var result = await _saidaDeCaminhaoRepository.RegistrarSaida(new RegistrarSaidaCaminhaoCommand
                {
                  Value = (int)input.PreRegistroId,
                  PreRegistroId = (int)input.PreRegistroId,
                    Direction = System.Data.ParameterDirection.Input
                }, (Domain.Enums.LocalPatio) input.Patio);

                return ServiceResult<bool>.Success(result);
            }
            catch (Exception exception)
            {
                return ServiceResult<bool>.Failure(exception.Message);
            }
        }
    }
}
