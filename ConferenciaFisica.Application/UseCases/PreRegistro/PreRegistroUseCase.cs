using System;
using ConferenciaFisica.Application.Common.Models;
using ConferenciaFisica.Application.ViewModels;
using ConferenciaFisica.Domain.Entities.PreRegistro;
using ConferenciaFisica.Domain.Enums;
using ConferenciaFisica.Domain.Repositories;

namespace ConferenciaFisica.Application.UseCases.PreRegistro
{
    public class PreRegistroUseCase : IPreRegistroUseCase
    {
        private readonly IPreRegistroRepository _preRegistroRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;

        public PreRegistroUseCase(IPreRegistroRepository preRegistroRepository, IAgendamentoRepository agendamentoRepository)
        {
            _preRegistroRepository = preRegistroRepository;
            _agendamentoRepository = agendamentoRepository;
        }

        public async Task<ServiceResult<bool>> Cadastrar(SaidaCaminhaoViewModel input)
        {
            var result = false;
            try
            {
                var hasPendenciaEntrada = await _agendamentoRepository.GetPendenciaEntrada(input.Placa, input.PlacaCarreta);

                if(hasPendenciaEntrada > 0)
                {
                    result = await _preRegistroRepository.AtualizarDataChegada(hasPendenciaEntrada);
                }
                else
                {
                    result = await _preRegistroRepository.Cadastrar(input.Protocolo,
                                                                            input.PlacaCarreta,
                                                                            input.PlacaCarreta,
                                                                            input.Ticket,
                                                                            input.Patio.HasValue ? LocalPatio.Patio : LocalPatio.Estacionamento,
                                                                            DateTime.Now,
                                                                            (DateTime?)DateTime.Now,
                                                                            !input.Patio.HasValue,
                                                                            input.FinalidadeId,
                                                                            input.PatioDestinoId
                                                                            );
                }
                  

                return ServiceResult<bool>.Success(result);
            }
            catch (Exception exception)
            {

                return ServiceResult<bool>.Failure(exception.Message);
            }
        }

        public async Task<ServiceResult<DadosAgendamentoModel>> GetDadosAgendamento(PreRegistroInput input)
        {
            try
            {
                if (input.LocalPatio == LocalPatio.Patio)
                {
                    var pendenciaSaida = await _agendamentoRepository.PendenciaDeSaidaEstacionamento(input.Placa, input.PlacaCarreta);
                    if (pendenciaSaida is not null)
                    {
                        return ServiceResult<DadosAgendamentoModel>.Failure($"Existe pendencia de saída no Estacionamento para a placa {input.Placa}");
                    }

                    var pendenciaDeSaidaPatio = await _agendamentoRepository.PendenciaDeSaidaPatio(input.Placa);
                    if (pendenciaDeSaidaPatio is not null)
                    {
                        return ServiceResult<DadosAgendamentoModel>.Failure($"Existe pendência de saída no Pátio para a placa {input.Placa}");
                    }
                }

                var dadosAgendamento = await _agendamentoRepository.GetDadosAgendamento(input.Sistema, input.Placa, input.PlacaCarreta);


                return ServiceResult<DadosAgendamentoModel>.Success(dadosAgendamento);
            }
            catch (Exception exception)
            {
                return ServiceResult<DadosAgendamentoModel>.Failure(exception.Message);
            }
        }
    }
}
